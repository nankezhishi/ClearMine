namespace ClearMine.UI.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Utilities;
    using ClearMine.Logic;
    using ClearMine.Media;
    using ClearMine.Properties;

    internal sealed class ClearMineViewModel : ViewModelBase
    {
        private IClearMine game = new ClearMineGame();

        #region NewGame Command
        private static CommandBinding newGameBinding = new CommandBinding(ApplicationCommands.New,
            new ExecutedRoutedEventHandler(OnNewGameExecuted), new CanExecuteRoutedEventHandler(OnNewGameCanExecuted));

        public static CommandBinding NewGameBinding
        {
            get { return newGameBinding; }
        }

        private static void OnNewGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>(vm => vm.game.StartNew());
        }

        private static void OnNewGameCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Refresh Binding
        private static CommandBinding refreshBinding = new CommandBinding(NavigationCommands.Refresh,
            new ExecutedRoutedEventHandler(OnRefreshExecuted), new CanExecuteRoutedEventHandler(OnRefreshCanExecute));

        public static CommandBinding RefreshBinding
        {
            get { return refreshBinding; }
        }

        private static void OnRefreshExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>(vm => vm.game.Restart());
        }

        private static void OnRefreshCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Close Command
        private static CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close,
            new ExecutedRoutedEventHandler(OnCloseExecuted), new CanExecuteRoutedEventHandler(OnCloseCanExecuted));

        public static CommandBinding CloseBinding
        {
            get { return closeBinding; }
        }

        private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>(vm => Application.Current.Shutdown());
        }

        private static void OnCloseCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Show Statistics Command
        private static ICommand showStatistics = new RoutedUICommand("Statistics", "Statistics",
            typeof(ClearMineViewModel), new InputGestureCollection() { new KeyGesture(Key.F4) });
        private static CommandBinding statisticsBinding = new CommandBinding(ShowStatistics,
            new ExecutedRoutedEventHandler(OnStatisticsExecuted), new CanExecuteRoutedEventHandler(OnStaisticsCanExecute));
        public static ICommand ShowStatistics
        {
            get { return showStatistics; }
        }

        public static CommandBinding StatisticsBinding
        {
            get { return statisticsBinding; }
        }

        private static void OnStatisticsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow(Settings.Default.Difficulty);
            statisticsWindow.Owner = Application.Current.MainWindow;
            statisticsWindow.ShowDialog();
        }

        private static void OnStaisticsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Option Command
        private static ICommand option = new RoutedUICommand("Option", "Option",
            typeof(ClearMineViewModel), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control) });
        private static CommandBinding optionBinding = new CommandBinding(Option,
            new ExecutedRoutedEventHandler(OnOptionExecuted), new CanExecuteRoutedEventHandler(OnOptionCanExecute));
        public static ICommand Option
        {
            get { return option; }
        }

        public static CommandBinding OptionBinding
        {
            get { return optionBinding; }
        }

        private static void OnOptionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>(vm =>
            {
                if (vm.game.GameState == GameState.Started)
                {
                    vm.game.Pause();
                }
            });
            var optionsWindow = new OptionsDialog();
            optionsWindow.Owner = Application.Current.MainWindow;
            if (optionsWindow.ShowDialog().Value)
            {
                e.ExtractDataContext<ClearMineViewModel>(vm =>
                {
                    if (vm.game.GameState == GameState.Initialized)
                    {
                        vm.Start();
                    }
                    else if (vm.game.GameState == GameState.Started)
                    {
                        vm.game.Resume();
                    }
                });
            }
        }

        private static void OnOptionCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region About Command
        private static ICommand about = new RoutedUICommand("About", "About", typeof(ClearMineViewModel));
        private static CommandBinding aboutBinding = new CommandBinding(About,
            new ExecutedRoutedEventHandler(OnAboutExecuted), new CanExecuteRoutedEventHandler(OnAboutCanExecute));
        public static ICommand About
        {
            get { return about; }
        }

        public static CommandBinding AboutBinding
        {
            get { return aboutBinding; }
        }

        private static void OnAboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var about = new AboutDialog();
            about.Owner = Application.Current.MainWindow;
            about.ShowDialog();
        }

        private static void OnAboutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Feeback Command
        private static ICommand feedback = new RoutedUICommand("Feedback", "Feedback", typeof(ClearMineViewModel));
        private static CommandBinding feedbackBinding = new CommandBinding(Feedback,
            new ExecutedRoutedEventHandler(OnFeedbackExecuted), new CanExecuteRoutedEventHandler(OnFeedbackCanExecute));
        public static ICommand Feedback
        {
            get { return feedback; }
        }

        public static CommandBinding FeedbackBinding
        {
            get { return feedbackBinding; }
        }

        private static void OnFeedbackExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private static void OnFeedbackCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        public ClearMineViewModel()
        {
            game.StateChanged += new EventHandler(OnGameStateChanged);
            game.TimeChanged += new EventHandler(OnGameTimeChanged);
            game.CellStateChanged += new EventHandler<CellStateChangedEventArgs>(OnCellStateChanged);
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
        }

        public int Columns
        {
            get { return (int)game.Size.Width; }
        }

        public int Rows
        {
            get { return (int)game.Size.Height; }
        }

        public int Time
        {
            get { return game.UsedTime / 1000; }
        }

        public int RemainedMines
        {
            get { return game.RemainedMines; }
        }

        public IEnumerable<MineCell> Cells
        {
            get { return game.Cells; }
        }

        public void Start()
        {
            try
            {
                InitialPlayground();
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
            OnPropertyChanged("Columns");
            OnPropertyChanged("Rows");
            OnPropertyChanged("RemainedMines");
            game.StartNew();
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
                OnPropertyChanged("RemainedMines");
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

        private void InitialPlayground()
        {
            game.Initialize(new Size(Settings.Default.Columns, Settings.Default.Rows), (int)Settings.Default.Mines);
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("Time");
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new[] { "Rows", "Columns", "Mines" }.Contains(e.PropertyName))
            {
                if (game.GameState == GameState.Initialized)
                {
                    InitialPlayground();
                    OnPropertyChanged(e.PropertyName);
                }
                else
                {
                    // Ignore it.
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
            OnPropertyChanged("RemainedMines");
            if (game.GameState == GameState.Failed)
            {
                Player.Play(@".\Sound\Lose.wma");
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateStatistics();
                    ShowLostWindow();
                }));
            }
            else if (game.GameState == GameState.Success)
            {
                Player.Play(@".\Sound\Win.wma");
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UpdateStatistics();
                    ShowWonWindow();
                }));
            }
            else if (game.GameState == GameState.Initialized)
            {
                Player.Play(@".\Sound\GameStart.wma");
            }
        }

        private void ShowLostWindow()
        {
            var lostWindow = new GameLostWindow();
            lostWindow.Owner = Application.Current.MainWindow;
            if ((bool)lostWindow.ShowDialog())
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
            var wonWindow = new GameWonWindow();
            wonWindow.Owner = Application.Current.MainWindow;
            if ((bool)wonWindow.ShowDialog())
            {
                Application.Current.Shutdown();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(a => game.StartNew());
            }
        }

        private void UpdateStatistics()
        {
            HeroHistory history = Settings.Default.HeroList.GetByLevel(Settings.Default.Difficulty);
            if (history != null)
            {
                if (game.GameState == GameState.Success)
                {
                    history.IncreaseWon(game.UsedTime / 1000, DateTime.Now);
                }
                else
                {
                    history.IncreaseLost();
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
                Player.Play(@".\Sound\TileMultiple.wma");
            }
            else
            {
                Player.Play(@".\Sound\TileSingle.wma");
            }
        }
    }
}
