﻿namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Logic;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Media;
    using ClearMine.Localization;
    using ClearMine.UI.Dialogs;
    using ClearMine.VM.Commands;
    using Microsoft.Win32;

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
                    vm.game.PauseGame();
                }
            });
            var optionsWindow = new OptionsDialog();
            optionsWindow.Owner = Application.Current.MainWindow;
            optionsWindow.DataContext = new OptionsViewModel();
            if (optionsWindow.ShowDialog().Value)
            {
                viewModel.StartNewGame();
            }
            else if (viewModel.game.GameState == GameState.Started)
            {
                viewModel.game.ResumeGame();
            }
        }

        private static void OnOptionCanExecute(object sender, CanExecuteRoutedEventArgs e)
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
            openFileDialog.Filter = LocalizationHelper.FindText("SavedGameFilter");
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

        public ClearMineViewModel()
        {
            HookupToGame(Infrastructure.Container.GetExportedValue<IClearMine>());
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

        public string Time
        {
            get { return ((double)game.UsedTime / 1000).ToString(CultureInfo.InvariantCulture); }
        }

        public string RemainedMines
        {
            get { return game.RemainedMines.ToString(CultureInfo.InvariantCulture); }
        }

        public Brush NewGameIcon
        {
            get
            {
                if (game.GameState == GameState.Initialized || game.GameState == GameState.Started)
                {
                    return Application.Current.FindResource("NormalFaceBrush") as Brush;
                }
                else if (game.GameState == GameState.Success)
                {
                    return Application.Current.FindResource("WinFaceBrush") as Brush;
                }
                else if (game.GameState == GameState.Failed)
                {
                    return Application.Current.FindResource("LosingFaceBrush") as Brush;
                }
                else
                {
                    throw new InvalidProgramException();
                }
            }
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
                if (Settings.Default.AlwaysNewGame ||
                    new ConfirmNewGameWindow() { Owner = Application.Current.MainWindow }.ShowDialog().Value)
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
                    game.ResumeGame();
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
                if (Settings.Default.SaveOnExit)
                {
                    SaveCurrentGame(@".\SavedGame.cmg");
                }
                else
                {
                    var result = MessageBox.Show(LocalizationHelper.FindText("AskingSaveGameMessage"), LocalizationHelper.FindText("AskingSaveGameTitle"),
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (result == MessageBoxResult.Yes)
                    {
                        SaveCurrentGame(@".\SavedGame.cmg");
                    }
                    else
                    {
                        UpdateStatistics();
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

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            yield return NewGameBinding;
            yield return OpenBinding;
            yield return OptionBinding;
            yield return RefreshBinding;
            yield return SaveAsBinding;
            yield return GameCommandBindings.AboutBinding;
            yield return GameCommandBindings.CloseBinding;
            yield return GameCommandBindings.FeedbackBinding;
            yield return GameCommandBindings.ShowLogBinding;
            yield return GameCommandBindings.StatisticsBinding;
            yield return GameCommandBindings.SwitchLanguageBinding;
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
            OnPropertyChanged("NewGameIcon");
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

        private static string TakeScreenShoot()
        {
            var target = VisualTreeHelper.GetChild(Application.Current.MainWindow, 0) as FrameworkElement;
            var targetBitmap = new RenderTargetBitmap((int)target.ActualWidth, (int)target.ActualHeight, 96d, 96d, PixelFormats.Default);
            targetBitmap.Render(target);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            string fileName = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture) + ".png";
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
            wonWindow.DataContext = new GameWonViewModel(game.UsedTime, DateTime.Now);
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
                    history.IncreaseWon(game.UsedTime / 1000.0, DateTime.Now, TakeScreenShoot());
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
                savePathDialog.Filter = LocalizationHelper.FindText("SavedGameFilter");
                if (savePathDialog.ShowDialog() == true)
                {
                    path = savePathDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            // Pause game to make sure the timestamp currect.
            game.PauseGame();
            var gameSaver = new XmlSerializer(game.GetType());
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

            IClearMine newgame = null;
            var gameLoader = new XmlSerializer(game.GetType());
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                newgame = (IClearMine)gameLoader.Deserialize(file);
            }
            if (newgame.CheckHash())
            {
                HookupToGame(newgame);
                RefreshUI();
                game.ResumeGame();
            }
            else
            {
                MessageBox.Show(LocalizationHelper.FindText("CorruptedSavedGameMessage"), LocalizationHelper.FindText("CorruptedSavedGameTitle"));
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
