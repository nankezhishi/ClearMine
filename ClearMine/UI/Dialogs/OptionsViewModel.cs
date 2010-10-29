namespace ClearMine.UI.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Framework.ComponentModel;
    using ClearMine.Framework.Utilities;
    using ClearMine.Properties;

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
            e.ExtractDataContext<OptionsViewModel>(vm => e.CanExecute = String.IsNullOrEmpty(vm.Error));
        } 
        #endregion

        public OptionsViewModel()
        {
            Settings.Default.Reload();
        }

        public Difficulty? Difficulty
        {
            get { return Settings.Default.Difficulty; }
            set
            {
                if (value.HasValue)
                {
                    Settings.Default.Difficulty = value.Value;

                    if (value.Value == UI.Difficulty.Beginner)
                    {
                        Rows = 9;
                        Columns = 9;
                        Mines = 10;
                    }
                    else if (value.Value == UI.Difficulty.Intermediate)
                    {
                        Rows = 16;
                        Columns = 16;
                        Mines = 40;
                    }
                    else if (value.Value == UI.Difficulty.Advanced)
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
                        Error = "Height should less than 24 and greater than 9.";
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
                        Error = "Width should less than 30 and greater than 9.";
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
                        Error = "Mines amount should less than the size of the area and greater than 10";
                    }
                    else
                    {
                        Error = null;
                    }
                }
                else
                {
                    Error = null;
                }

                return Error;
            }
        }
    }
}
