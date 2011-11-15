namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.VM.Commands;

    internal sealed class OptionsViewModel : ViewModelBase, IDataErrorInfo
    {
        private string error;

        public OptionsViewModel()
        {
            Settings.Default.PropertyChanged += OnSettingsChanged;
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

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int Rows
        {
            get { return Settings.Default.Rows; }
            set { Settings.Default.Rows = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int Columns
        {
            get { return Settings.Default.Columns; }
            set { Settings.Default.Columns = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public int Mines
        {
            get { return Settings.Default.Mines; }
            set { Settings.Default.Mines = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool PlaySound
        {
            get { return Settings.Default.PlaySound; }
            set { Settings.Default.PlaySound = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool PlayAnimation
        {
            get { return Settings.Default.PlayAnimation; }
            set { Settings.Default.PlayAnimation = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool WavingFlag
        {
            get { return Settings.Default.WavingFlag; }
            set { Settings.Default.WavingFlag = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool ShowQuestionMark
        {
            get { return Settings.Default.ShowQuestionMark; }
            set { Settings.Default.ShowQuestionMark = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool ShowTooManyFlagsWarning
        {
            get { return Settings.Default.ShowTooManyFlagsWarning; }
            set { Settings.Default.ShowTooManyFlagsWarning = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool SaveOnExit
        {
            get { return Settings.Default.SaveOnExit; }
            set { Settings.Default.SaveOnExit = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool AutoContinueSaved
        {
            get { return Settings.Default.AutoContinueSaved; }
            set { Settings.Default.AutoContinueSaved = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool AlwaysNewGame
        {
            get { return Settings.Default.AlwaysNewGame; }
            set { Settings.Default.AlwaysNewGame = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool SaveGame
        {
            get { return Settings.Default.SaveGame; }
            set { Settings.Default.SaveGame = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string GameHistoryFolder
        {
            get { return Settings.Default.GameHistoryFolder; }
            set { Settings.Default.GameHistoryFolder = value; }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void Save()
        {
            Settings.Default.Save();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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
                        Error = ResourceHelper.FindText("InvalidHeightMessage");
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
                        Error = ResourceHelper.FindText("InvalidWidthMessage");
                    }
                    else
                    {
                        Error = null;
                    }
                }
                else if ("Mines".Equals(propertyName, StringComparison.Ordinal))
                {
                    if (Mines < 10 || Mines > Rows * Columns)
                    {
                        Error = ResourceHelper.FindText("InvalidMinesMessage");
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
                        Error = ResourceHelper.FindText("HistoryNotExistMessage");
                    }
                }
                else
                {
                    Error = null;
                }

                return Error;
            }
        }

        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.OptionCommandBindings; }
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            TriggerPropertyChanged(e.PropertyName);
        }
    }
}
