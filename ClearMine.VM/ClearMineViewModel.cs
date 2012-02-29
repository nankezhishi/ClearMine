namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Model;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Messages;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Messages;
    using ClearMine.GameDefinition.Utilities;
    using ClearMine.VM.Commands;

    public sealed class ClearMineViewModel : ViewModelBase
    {
        private IClearMine game;
        private bool pandingInitialize = true;
        private bool isMousePressed = false;
        private double itemSize;

        public ClearMineViewModel()
        {
            UpdateWindowSizeAccordingToDifficulty();
            Settings.Default.PropertyChanged += OnSettingsChanged;
            MessageManager.SubscribeMessage<GameLoadMessage>(OnGameLoaded);
            MessageManager.SubscribeMessage<CellStateMessage>(OnCellStatusChanged);
            MessageManager.SubscribeMessage<GameStateMessage>(OnGameStatusChanged);
            OnGameLoaded(new GameLoadMessage(Infrastructure.Container.GetExportedValue<IClearMine>()));
        }

        [ReadOnly(true)]
        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.MainCommandBindings; }
        }

        [ReadOnly(true)]
        public int Columns
        {
            get { return (int)game.Size.Width; }
        }

        [ReadOnly(true)]
        public int Rows
        {
            get { return (int)game.Size.Height; }
        }

        public double Width { get; set; }

        public double ItemSize
        {
            get { return itemSize; }
            set { SetProperty(ref itemSize, value, "ItemSize"); }
        }

        [ReadOnly(true)]
        public string Time
        {
            get { return Convert.ToString(game.UsedTime / 1000d, CultureInfo.InvariantCulture); }
        }

        [ReadOnly(true)]
        public TimeSpan DigitalTime
        {
            get { return new TimeSpan(0, 0, 0, 0, game.UsedTime); }
        }

        [ReadOnly(true)]
        public string RemainedMines
        {
            get { return Convert.ToString(game.RemainedMines, CultureInfo.InvariantCulture); }
        }

        [ReadOnly(true)]
        public int TotalMines
        {
            get { return game.TotalMines; }
        }

        [ReadOnly(true)]
        public string Flags
        {
            get { return Convert.ToString(game.Cells.Where(c => c.State == CellState.MarkAsMine).Count(), CultureInfo.InvariantCulture); }
        }

        public bool IsMousePressed
        {
            get { return isMousePressed; }
            set
            {
                if (isMousePressed != value)
                {
                    isMousePressed = value;
                    TriggerPropertyChanged("IsMousePressed");
                }
            }
        }

        [ReadOnly(true)]
        public bool IsPaused
        {
            get { return game.GameState == GameState.Paused; }
        }

        [ReadOnly(true)]
        public GameState State
        {
            get { return game.GameState; }
        }

        [ReadOnly(true)]
        public int TimeAccuracy
        {
            get { return Settings.Default.AccurateTime ? 1 : 0; }
        }

        public IEnumerable<MineCell> Cells
        {
            get { return game.Cells; }
        }

        internal IClearMine Game
        {
            get { return game; }
        }

        public void StartNewGame()
        {
            if (game.GameState == GameState.Started || game.GameState == GameState.Paused)
            {
                if (Settings.Default.AlwaysNewGame || ShowDialog(PopupDialog.ConfirmNewGame))
                {
                    if (pandingInitialize)
                    {
                        Initialize();
                        RefreshUI();
                    }

                    game.UpdateStatistics();
                    game.StartNew();
                }
                else
                {
                    game.ResumeGame();
                }
            }
            else
            {
                if (pandingInitialize)
                {
                    Initialize();
                }

                game.StartNew();
            }
        }

        public void RefreshUI()
        {
            TriggerPropertyChanged("Columns");
            TriggerPropertyChanged("DigitalTime");
            TriggerPropertyChanged("Flags");
            TriggerPropertyChanged("Rows");
            TriggerPropertyChanged("RemainedMines");
            TriggerPropertyChanged("Time");
        }

        private void Initialize()
        {
            try
            {
                InitialPlayground();
                pandingInitialize = false;
            }
            catch (InvalidOperationException)
            {
                // While, there probably something wrong with your configurations.
                Settings.Default.Rows = 9;
                Settings.Default.Columns = 9;
                Settings.Default.Mines = 10;
                Settings.Default.Difficulty = Difficulty.Beginner;

                // Try again.
                InitialPlayground();
            }
        }

        private void InitialPlayground()
        {
            game.Initialize(new Size(Settings.Default.Columns, Settings.Default.Rows), (int)Settings.Default.Mines);
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new[] { "Rows", "Columns", "Mines" }.Contains(e.PropertyName))
            {
                if (game.GameState == GameState.Initialized)
                {
                    Initialize();
                    TriggerPropertyChanged(e.PropertyName);
                }
                else
                {
                    pandingInitialize = true;
                }
            }
            else if ("AccurateTime".Equals(e.PropertyName, StringComparison.Ordinal))
            {
                TriggerPropertyChanged("TimeAccuracy");
            }
            else if ("Difficulty".Equals(e.PropertyName, StringComparison.Ordinal))
            {
                UpdateWindowSizeAccordingToDifficulty();
            }
            else
            {
                // Ignore it.
            }
        }

        private void UpdateWindowSizeAccordingToDifficulty()
        {
            if (Settings.Default.Difficulty == Difficulty.Beginner)
            {
                Width = 450;
            }
            else if (Settings.Default.Difficulty == Difficulty.Intermediate)
            {
                Width = 480;
            }
            else if (Settings.Default.Difficulty == Difficulty.Advanced)
            {
                Width = 900;
            }
            else
            {
                Width = 30 * Settings.Default.Columns;
                if (Width < 450)
                    Width = 450;
            }

            TriggerPropertyChanged("Width");
        }

        private void OnCellStatusChanged(CellStateMessage message)
        {
            if (Settings.Default.PlayAnimation &&
                message.Cell.State == CellState.Shown &&
                Game.GameState == GameState.Started)
            {
                Thread.Sleep(1);
            }
        }

        private void OnGameStatusChanged(GameStateMessage message)
        {
            TriggerPropertyChanged("IsPaused");
            TriggerPropertyChanged("RemainedMines");
            TriggerPropertyChanged("Flags");
            TriggerPropertyChanged("State");
            if (game.GameState == GameState.Failed)
            {
                IsMousePressed = false;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    game.UpdateStatistics();
                    ShowLostWindow();
                }));
            }
            else if (game.GameState == GameState.Success)
            {
                IsMousePressed = false;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    game.UpdateStatistics();
                    ShowWonWindow();
                }), DispatcherPriority.Input);
            }
        }

        private bool ShowLostWindow()
        {
            return ShowDialog(PopupDialog.GameLost) ?
                   ThreadPool.QueueUserWorkItem(a => game.StartNew()) :
                   ThreadPool.QueueUserWorkItem(a => game.Restart());
        }

        private void ShowWonWindow()
        {
            if (ShowDialog(PopupDialog.GameWon, new GameWonViewModel(game.UsedTime, DateTime.Now)))
            {
                Application.Current.Shutdown();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(a => game.StartNew());
            }
        }

        private static bool ShowDialog(PopupDialog type, object data = null)
        {
            return (bool)MessageManager.SendMessage<ShowDialogMessage>(type, data);
        }

        private void OnGameLoaded(GameLoadMessage message)
        {
            if (message.NewGame != null)
            {
                if (game == null)
                {
                    game = message.NewGame;
                    game.TimeChanged += (sender, e) =>
                    {
                        TriggerPropertyChanged("Time");
                        TriggerPropertyChanged("DigitalTime");
                    };
                }
                else
                {
                    game.Update(message.NewGame);
                }

                RefreshUI();
            }
        }
    }
}
