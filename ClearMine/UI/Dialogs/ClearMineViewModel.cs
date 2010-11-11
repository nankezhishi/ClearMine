namespace ClearMine.UI.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Utilities;
    using ClearMine.Logic;
    using ClearMine.Media;
    using ClearMine.Properties;
    using Microsoft.Win32;
    using System.Diagnostics;

    internal sealed class ClearMineViewModel : ViewModelBase
    {
        private IClearMine game;
        private bool pandingInitialize = true;
        private double itemSize;

        #region NewGame Command
        private static CommandBinding newGameBinding = new CommandBinding(ApplicationCommands.New,
            new ExecutedRoutedEventHandler(OnNewGameExecuted), new CanExecuteRoutedEventHandler(OnNewGameCanExecuted));

        public static CommandBinding NewGameBinding
        {
            get { return newGameBinding; }
        }

        private static void OnNewGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>().StartNewGame();
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
            e.ExtractDataContext<ClearMineViewModel>().game.Restart();
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
            Application.Current.MainWindow.Close();
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
            var viewModel =  e.ExtractDataContext<ClearMineViewModel>(vm =>
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
                viewModel.StartNewGame();
            }
            else if (viewModel.game.GameState == GameState.Started)
            {
                viewModel.game.Resume();
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
        #region SaveAsCommand
        private static CommandBinding saveAsBinding = new CommandBinding(ApplicationCommands.SaveAs,
            new ExecutedRoutedEventHandler(OnSaveAsExecuted), new CanExecuteRoutedEventHandler(OnSaveAsCanExecute));

        public static CommandBinding SaveAsBinding
        {
            get { return saveAsBinding; }
        }

        private static void OnSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>().SaveCurrentGame();
        }

        private static void OnSaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.ExtractDataContext<ClearMineViewModel>().game.GameState == GameState.Started;
        } 
        #endregion
        #region OpenCommand
        private static CommandBinding openBinding = new CommandBinding(ApplicationCommands.Open,
            new ExecutedRoutedEventHandler(OnOpenExecuted), new CanExecuteRoutedEventHandler(OnOpenCanExecute));

        public static CommandBinding OpenBinding
        {
            get { return openBinding; }
        }

        private static void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".cmg";
            openFileDialog.Filter = "ClearMine Saved Game File (*.cmg)|*.cmg";
            if (openFileDialog.ShowDialog() == true)
            {
                e.ExtractDataContext<ClearMineViewModel>().LoadSavedGame(openFileDialog.FileName);
            }
        }

        private static void OnOpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }  
        #endregion
        #region ShowLogCommand
        private static ICommand showLog = new RoutedUICommand("ShowLog", "ShowLog",
            typeof(ClearMineViewModel), new InputGestureCollection() { new KeyGesture(Key.L, ModifierKeys.Control) });
        private static CommandBinding showLogBinding = new CommandBinding(ShowLog,
            new ExecutedRoutedEventHandler(OnShowLogExecuted), new CanExecuteRoutedEventHandler(OnShowLogCanExecute));
        public static ICommand ShowLog
        {
            get { return showLog; }
        }

        public static CommandBinding ShowLogBinding
        {
            get { return showLogBinding; }
        }

        private static void OnShowLogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var outputWindow = new OutputWindow();
            outputWindow.Owner = Application.Current.MainWindow;
            outputWindow.Show();
        }

        private static void OnShowLogCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }  
        #endregion

        public ClearMineViewModel()
        {
            HookupToGame(new ClearMineGame());
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

        public double ItemSize
        {
            get { return itemSize; }
            set { SetProperty(ref itemSize, value, "ItemSize"); }
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

        public void StartNewGame()
        {
            if (game.GameState != GameState.Started)
            {
                if (pandingInitialize)
                {
                    Initialize();
                }
                game.StartNew();
            }
            else if (game.GameState == GameState.Started)
            {
                var confirm = new ConfirmNewGameWindow();
                confirm.Owner = Application.Current.MainWindow;
                if (confirm.ShowDialog().Value)
                {
                    if (pandingInitialize)
                    {
                        Initialize();
                        RefreshUI();
                    }
                    UpdateStatistics();
                    game.StartNew();
                }
                else
                {
                    game.Resume();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void RequestToClose(CancelEventArgs e)
        {
            if (game.GameState == GameState.Started)
            {
                var result =  MessageBox.Show("Do you want to save the game?", "Clear Mine - Save",
                                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (Settings.Default.SaveOnExit || result == MessageBoxResult.Yes)
                {
                    SaveCurrentGame(@".\SavedGame.cmg");
                }
                else
                {
                    UpdateStatistics();
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

        public void RefreshUI()
        {
            OnPropertyChanged("Columns");
            OnPropertyChanged("Rows");
            OnPropertyChanged("RemainedMines");
            OnPropertyChanged("Time");
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
            OnPropertyChanged("Time");
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new[] { "Rows", "Columns", "Mines" }.Contains(e.PropertyName))
            {
                if (game.GameState == GameState.Initialized)
                {
                    Initialize();
                    OnPropertyChanged(e.PropertyName);
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
                }), DispatcherPriority.Input);
            }
            else if (game.GameState == GameState.Initialized)
            {
                Player.Play(@".\Sound\GameStart.wma");
            }
        }

        private string TakeScreenShoot()
        {
            var target = VisualTreeHelper.GetChild(Application.Current.MainWindow, 0) as FrameworkElement;
            var targetBitmap = new RenderTargetBitmap((int)target.ActualWidth, (int)target.ActualHeight, 96d, 96d, PixelFormats.Default);
            targetBitmap.Render(target);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            string fileName = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss") + ".png";
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Poleaf\ClearMine\ScreenShoots\";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            fileName = Path.Combine(folder, fileName);

            // save file to disk
            using (FileStream fs = File.Open(fileName, FileMode.OpenOrCreate))
            {
                encoder.Save(fs);
            }

            Trace.TraceInformation("Screen shoot saved to {0}", fileName);

            return fileName;
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
                    history.IncreaseWon(game.UsedTime / 1000, DateTime.Now, TakeScreenShoot());
                }
                else if (game.GameState == GameState.Failed)
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
                Player.Play(@".\Sound\TileMultiple.wma");
            }
            else
            {
                Player.Play(@".\Sound\TileSingle.wma");
            }
        }

        private void SaveCurrentGame(string path = null)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                var savePathDialog = new SaveFileDialog();
                savePathDialog.DefaultExt = ".cmg";
                savePathDialog.Filter = "ClearMine Saved Game File (*.cmg)|*.cmg";
                if (savePathDialog.ShowDialog() == true)
                {
                    path = savePathDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            var gameSaver = new XmlSerializer(typeof(ClearMineGame));
            using (var file = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                gameSaver.Serialize(file, game);
            }
        }

        private void LoadSavedGame(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Cannot open game file", path);
            }

            ClearMineGame newgame = null;
            var gameLoader = new XmlSerializer(typeof(ClearMineGame));
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                newgame = (ClearMineGame)gameLoader.Deserialize(file);
            }
            if (newgame.CheckHash())
            {
                HookupToGame(newgame);
                RefreshUI();
                game.Resume();
            }
            else
            {
                MessageBox.Show("The saved game has been modified to an incorrect state. Please try to fix it and open again.", "Clear Mine - Corrupted game file");
            }
        }

        private void HookupToGame(IClearMine newgame)
        {
            if (game == null)
            {
                game = newgame;

                game.StateChanged += new EventHandler(OnGameStateChanged);
                game.TimeChanged += new EventHandler(OnGameTimeChanged);
                game.CellStateChanged += new EventHandler<CellStateChangedEventArgs>(OnCellStateChanged);
            }
            else
            {
                game.Update(newgame);
            }
        }
    }
}
