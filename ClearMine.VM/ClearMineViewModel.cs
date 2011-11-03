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
    using System.Windows.Media;
    using System.Windows.Threading;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Messages;
    using ClearMine.GameDefinition;
    using ClearMine.VM.Commands;

    internal sealed class ClearMineViewModel : ViewModelBase
    {
        private IClearMine game;
        private bool pandingInitialize = true;
        private bool isMousePressed = false;
        private double itemSize;

        public ClearMineViewModel()
        {
            Settings.Default.PropertyChanged += OnSettingsChanged;
            MessageManager.GetMessageAggregator<GameLoadMessage>().Subscribe(OnGameLoaded);
            OnGameLoaded(new GameLoadMessage() { NewGame = Infrastructure.Container.GetExportedValue<IClearMine>() });
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
        public string RemainedMines
        {
            get { return Convert.ToString(game.RemainedMines, CultureInfo.InvariantCulture); }
        }

        public bool IsMousePressed
        {
            get { return isMousePressed; }
            set
            {
                if (isMousePressed != value)
                {
                    isMousePressed = value;
                    TriggerPropertyChanged("NewGameIcon");
                }
            }
        }

        [ReadOnly(true)]
        public bool IsPaused
        {
            get { return game.GameState == GameState.Paused; }
        }

        public Brush NewGameIcon
        {
            get
            {
                if (IsMousePressed)
                {
                    return Application.Current.FindResource("TryFaceBrush") as Brush;
                }

                if (game.GameState == GameState.Success)
                {
                    return Application.Current.FindResource("WinFaceBrush") as Brush;
                }
                else if (game.GameState == GameState.Failed)
                {
                    return Application.Current.FindResource("LosingFaceBrush") as Brush;
                }
                else
                {
                    return Application.Current.FindResource("NormalFaceBrush") as Brush;
                }
            }
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
                if (Settings.Default.AlwaysNewGame ||
                    ShowDialog("ClearMine.UI.Dialogs.ConfirmNewGameWindow, ClearMine.Dialogs"))
                {
                    if (pandingInitialize)
                    {
                        Initialize();
                        RefreshUI();
                    }

                    UpdateStatistics(game.GameState);
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

        public void RequestToClose(CancelEventArgs e)
        {
            if (game.GameState == GameState.Started)
            {
                if (Settings.Default.SaveOnExit)
                {
                    Infrastructure.Container.GetExportedValue<IGameSerializer>().SaveGame(game, Settings.Default.UnfinishedGameFileName);
                }
                else
                {
                    // Game shouldn't be paused here
                    var result = MessageBox.Show(LocalizationHelper.FindText("AskingSaveGameMessage"), LocalizationHelper.FindText("AskingSaveGameTitle"),
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (result == MessageBoxResult.Yes)
                    {
                        Infrastructure.Container.GetExportedValue<IGameSerializer>().SaveGame(game, Settings.Default.UnfinishedGameFileName);
                    }
                    else
                    {
                        UpdateStatistics(game.GameState);
                    }
                }
            }
        }

        public void MarkAt(MineCell cell)
        {
            if (cell != null && (game.GameState == GameState.Initialized || game.GameState == GameState.Started))
            {
                if (cell.State == CellState.Normal)
                {
                    game.TryMarkAt(cell, CellState.MarkAsMine);
                }
                else if (cell.State == CellState.MarkAsMine)
                {
                    if (Settings.Default.ShowQuestionMark)
                    {
                        game.TryMarkAt(cell, CellState.Question);
                    }
                    else
                    {
                        game.TryMarkAt(cell, CellState.Normal);
                    }
                }
                else if (cell.State == CellState.Question)
                {
                    game.TryMarkAt(cell, CellState.Normal);
                }
                else
                {
                    // Do nothing.
                }

                TriggerPropertyChanged("RemainedMines");
            }
        }

        public void DigAt(MineCell cell)
        {
            if (cell != null && (game.GameState == GameState.Initialized || game.GameState == GameState.Started))
            {
                ThreadPool.QueueUserWorkItem(o => HandleExpandedCells(game.TryDigAt(cell)));
            }
        }

        public void TryExpand(MineCell cell)
        {
            if (cell != null && (game.GameState == GameState.Initialized || game.GameState == GameState.Started))
            {
                ThreadPool.QueueUserWorkItem(o => HandleExpandedCells(game.TryExpandAt(cell)));
            }
        }

        public void RefreshUI()
        {
            TriggerPropertyChanged("Columns");
            TriggerPropertyChanged("Rows");
            TriggerPropertyChanged("RemainedMines");
            TriggerPropertyChanged("Time");
        }

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            return GameCommandBindings.GetGameCommandBindings();
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
                Settings.Default.Save();

                // Try again.
                InitialPlayground();
            }
        }

        private void InitialPlayground()
        {
            game.Initialize(new Size(Settings.Default.Columns, Settings.Default.Rows), (int)Settings.Default.Mines);
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            TriggerPropertyChanged("Time");
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
            else
            {
                // Ignore it.
            }
        }

        private void OnCellStateChanged(object sender, CellStateChangedEventArgs e)
        {
            if (Settings.Default.PlayAnimation)
            {
                if (e.Cell.State == CellState.Shown)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void OnGameStateChanged(object sender, EventArgs e)
        {
            TriggerPropertyChanged("IsPaused");
            TriggerPropertyChanged("RemainedMines");
            TriggerPropertyChanged("NewGameIcon");
            if (game.GameState == GameState.Failed)
            {
                IsMousePressed = false;
                Player.Play(Settings.Default.SoundLose);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateStatistics(game.GameState);
                    ShowLostWindow();
                }));
            }
            else if (game.GameState == GameState.Success)
            {
                IsMousePressed = false;
                Player.Play(Settings.Default.SoundWin);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateStatistics(game.GameState);
                    ShowWonWindow();
                }), DispatcherPriority.Input);
            }
            else if (game.GameState == GameState.Initialized)
            {
                Player.Play(Settings.Default.SoundStart);
            }
        }

        private void ShowLostWindow()
        {
            if (ShowDialog("ClearMine.UI.Dialogs.GameLostWindow, ClearMine.Dialogs"))
            {
                ThreadPool.QueueUserWorkItem(a => game.StartNew());
            }
            else
            {
                ThreadPool.QueueUserWorkItem(a => game.Restart());
            }
        }

        private void ShowWonWindow()
        {
            if (ShowDialog("ClearMine.UI.Dialogs.GameWonWindow, ClearMine.Dialogs", new GameWonViewModel(game.UsedTime, DateTime.Now)))
            {
                Application.Current.Shutdown();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(a => game.StartNew());
            }
        }

        private bool ShowDialog(string type, object data = null)
        {
            var message = new ShowDialogMessage()
            {
                DialogType = Type.GetType(type),
                Data = data,
            };
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(message);

            return (bool)message.HandlingResult;
        }

        private void UpdateStatistics(GameState state)
        {
            var history = Settings.Default.HeroList.GetByLevel(Settings.Default.Difficulty);
            if (history != null)
            {
                if (state == GameState.Success)
                {
                    var target = VisualTreeHelper.GetChild(Application.Current.MainWindow, 0) as FrameworkElement;
                    var filePath = Infrastructure.Container.GetExportedValue<IVisualShoot>().SaveSnapShoot(target);

                    history.IncreaseWon(game.UsedTime / 1000.0, DateTime.Now, filePath);
                }
                else if (state == GameState.Failed)
                {
                    history.IncreaseLost();
                }
                else
                {
                    history.IncreaseUndone();
                }
            }

            Settings.Default.Save();
        }

        private static void HandleExpandedCells(IEnumerable<MineCell> cells)
        {
            int emptyCellExpanded = cells.Count(c => c.MinesNearby == 0);
            if (emptyCellExpanded == 0)
            {
                // Do nothing.
            }
            else if (emptyCellExpanded > 1)
            {
                Player.Play(Settings.Default.SoundTileMultiple);
            }
            else
            {
                Player.Play(Settings.Default.SoundTileSingle);
            }
        }

        private void OnGameLoaded(GameLoadMessage message)
        {
            if (message.NewGame != null)
            {
                if (game == null)
                {
                    game = message.NewGame;

                    game.StateChanged += OnGameStateChanged;
                    game.TimeChanged += OnGameTimeChanged;
                    game.CellStateChanged += OnCellStateChanged;
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
