namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Commands;
    using ClearMine.Localization;

    internal sealed class OptionsViewModel : ViewModelBase, IDataErrorInfo
    {
        private string error;

        #region Close Command
        private static CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close,
            new ExecutedRoutedEventHandler(OnCloseExecuted), new CanExecuteRoutedEventHandler(OnCloseCanExecute));
        public static CommandBinding CloseBinding
        {
            get { return closeBinding; }
        }

        private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<OptionsViewModel>(vm =>
            {
                vm.Cancel();
                var window = (sender as DependencyObject).FindAncestor<Window>();
                if (window != null)
                {
                    window.DialogResult = false;
                    window.Close();
                }
            });
        }

        private static void OnCloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        } 
        #endregion

        #region Save Command
        private static CommandBinding saveBinding = new CommandBinding(ApplicationCommands.Save,
            new ExecutedRoutedEventHandler(OnSaveExecuted), new CanExecuteRoutedEventHandler(OnSaveCanExecuted));

        public static CommandBinding SaveBinding
        {
            get { return saveBinding; }
        }

        private static void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<OptionsViewModel>(vm =>
            {
                vm.Save();
                var window = (sender as DependencyObject).FindAncestor<Window>();
                if (window != null)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            });
        }

        private static void OnSaveCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = String.IsNullOrWhiteSpace(e.ExtractDataContext<OptionsViewModel>().Error);
        } 
        #endregion

        #region BrowseHistory Command
        private static CommandBinding browseHistoryBinding = new CommandBinding(OptionsCommands.BrowseHistory,
            new ExecutedRoutedEventHandler(OnBrowseHistoryExecuted), new CanExecuteRoutedEventHandler(OnBrowseHistoryCanExecute));

        public static CommandBinding BrowseHistoryBinding
        {
            get { return browseHistoryBinding; }
        }

        private static void OnBrowseHistoryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.Description = LocalizationHelper.FindText("BrowseGameFolderMessage");
            folderBrowser.SelectedPath = Path.GetFullPath(Settings.Default.GameHistoryFolder);
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.GameHistoryFolder = folderBrowser.SelectedPath;
            }
        }

        private static void OnBrowseHistoryCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Settings.Default.SaveGame;
        }
        
        #endregion

        public OptionsViewModel()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
            //Settings.Default.Reload();
        }

        public Difficulty? Difficulty
        {
            get { return Settings.Default.Difficulty; }
            set
            {
                if (value.HasValue)
                {
                    Settings.Default.Difficulty = value.Value;

                    if (value.Value == Common.Difficulty.Beginner)
                    {
                        Rows = 9;
                        Columns = 9;
                        Mines = 10;
                    }
                    else if (value.Value == Common.Difficulty.Intermediate)
                    {
                        Rows = 16;
                        Columns = 16;
                        Mines = 40;
                    }
                    else if (value.Value == Common.Difficulty.Advanced)
                    {
                        Rows = 16;
                        Columns = 30;
                        Mines = 99;
                    }
                    else
                    {
                        // Do nothing.
                    }
                }
            }
        }

        public uint Rows
        {
            get { return Settings.Default.Rows; }
            set { Settings.Default.Rows = value; }
        }

        public uint Columns
        {
            get { return Settings.Default.Columns; }
            set { Settings.Default.Columns = value; }
        }

        public uint Mines
        {
            get { return Settings.Default.Mines; }
            set { Settings.Default.Mines = value; }
        }

        public bool PlaySound
        {
            get { return Settings.Default.PlaySound; }
            set { Settings.Default.PlaySound = value; }
        }

        public bool PlayAnimation
        {
            get { return Settings.Default.PlayAnimation; }
            set { Settings.Default.PlayAnimation = value; }
        }

        public bool ShowQuestionMark
        {
            get { return Settings.Default.ShowQuestionMark; }
            set { Settings.Default.ShowQuestionMark = value; }
        }

        public bool SaveOnExit
        {
            get { return Settings.Default.SaveOnExit; }
            set { Settings.Default.SaveOnExit = value; }
        }

        public bool AutoContinueSaved
        {
            get { return Settings.Default.AutoContinueSaved; }
            set { Settings.Default.AutoContinueSaved = value; }
        }

        public bool AlwaysNewGame
        {
            get { return Settings.Default.AlwaysNewGame; }
            set { Settings.Default.AlwaysNewGame = value; }
        }

        public bool SaveGame
        {
            get { return Settings.Default.SaveGame; }
            set { Settings.Default.SaveGame = value; }
        }

        public string GameHistoryFolder
        {
            get { return Settings.Default.GameHistoryFolder; }
            set { Settings.Default.GameHistoryFolder = value; }
        }

        public void Save()
        {
            Settings.Default.Save();
        }

        public void Cancel()
        {
            Settings.Default.Reload();
        }

        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value, "Error"); }
        }

        public string this[string propertyName]
        {
            get
            {
                if ("Rows".Equals(propertyName, StringComparison.Ordinal))
                {
                    if (Rows < 9 || Rows > 24)
                    {
                        Error = LocalizationHelper.FindText("InvalidHeightMessage");
                    }
                    else
                    {
                        Error = null;
                    }
                }
                else if ("Columns".Equals(propertyName, StringComparison.Ordinal))
                {
                    if (Columns < 9 || Columns > 30)
                    {
                        Error = LocalizationHelper.FindText("InvalidWidthMessage");
                    }
                    else
                    {
                        Error = null;
                    }
                }
                else if("Mines".Equals(propertyName, StringComparison.Ordinal))
                {
                    if (Mines < 10 || Mines > Rows * Columns)
                    {
                        Error = LocalizationHelper.FindText("InvalidMinesMessage");
                    }
                    else
                    {
                        Error = null;
                    }
                }
                else if ("GameHistoryFolder".Equals(propertyName, StringComparison.Ordinal))
                {
                    if (Directory.Exists(GameHistoryFolder))
                    {
                        Error = null;
                    }
                    else
                    {
                        Error = LocalizationHelper.FindText("HistoryNotExistMessage");
                    }
                }
                else
                {
                    Error = null;
                }

                return Error;
            }
        }

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            yield return SaveBinding;
            yield return CloseBinding;
            yield return BrowseHistoryBinding;
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
