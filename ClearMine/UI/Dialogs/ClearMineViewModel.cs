namespace ClearMine.UI.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Framework.ComponentModel;
    using ClearMine.Framework.Utilities;
    using ClearMine.Logic;
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
            var statisticsWindow = new StatisticsWindow();
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
            var optionsWindow = new OptionsDialog();
            optionsWindow.Owner = Application.Current.MainWindow;
            // Start a new game if user click OK button.
            if (optionsWindow.ShowDialog().Value)
            {
                e.ExtractDataContext<ClearMineViewModel>( vm => vm.game.StartNew() );
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

        public ClearMineViewModel()
        {
            game.StateChanged += new EventHandler(OnGameStateChanged);
            game.TimeChanged += new EventHandler(OnGameTimeChanged);
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
                Settings.Default.Difficulty = Enum.GetName(typeof(Difficulty), Difficulty.Beginner);
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
            if (cell != null)
            {
                if (cell.State == CellState.Normal)
                    game.TryMarkAt(cell, CellState.MarkAsMine);
                else if (cell.State == CellState.MarkAsMine)
                    game.TryMarkAt(cell, CellState.Question);
                else if (cell.State == CellState.Question)
                    game.TryMarkAt(cell, CellState.Normal);
                else
                    // Ignore rest.
                OnPropertyChanged("RemainedMines");
            }
        }

        public void DigAt(MineCell cell)
        {
            if (cell != null)
            {
                game.TryDigAt(cell);
            }
        }

        public void TryExpand(MineCell cell)
        {
            if (cell != null)
            {
                game.TryExpandAt(cell);
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
                InitialPlayground();
                OnPropertyChanged(e.PropertyName);
            }
            else
            {
                // Ignore it.
            }
        }

        private void OnGameStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("RemainedMines");
            if (game.GameState == GameState.Failed)
            {
                var lostWindow = new GameLostWindow();
                lostWindow.Owner = Application.Current.MainWindow;
                if ((bool)lostWindow.ShowDialog())
                {
                    game.StartNew();
                }
                else
                {
                    game.Restart();
                }
            }
            else if (game.GameState == GameState.Success)
            {
                var wonWindow = new GameWonWindow();
                wonWindow.Owner = Application.Current.MainWindow;
                if ((bool)wonWindow.ShowDialog())
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    game.StartNew();
                }
            }
        }
    }
}
