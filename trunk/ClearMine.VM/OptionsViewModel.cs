namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Model;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.VM.Commands;

    /// <summary>
    /// 
    /// </summary>
    public sealed class OptionsViewModel : DynamicObject, IDataErrorInfo, ITransaction, IViewModel, INotifyPropertyChanged
    {
        private string error;
        private static readonly IEnumerable<string> settingsMembers = typeof(Settings).GetProperties().Select(p => p.Name);

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

                    if (value.Value == Common.Model.Difficulty.Beginner)
                    {
                        Settings.Default.Rows = 9;
                        Settings.Default.Columns = 9;
                        Settings.Default.Mines = 10;
                    }
                    else if (value.Value == Common.Model.Difficulty.Intermediate)
                    {
                        Settings.Default.Rows = 16;
                        Settings.Default.Columns = 16;
                        Settings.Default.Mines = 40;
                    }
                    else if (value.Value == Common.Model.Difficulty.Advanced)
                    {
                        Settings.Default.Rows = 16;
                        Settings.Default.Columns = 30;
                        Settings.Default.Mines = 99;
                    }
                    else
                    {
                        // Do nothing.
                    }
                }
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder != null && settingsMembers.Contains(binder.Name))
            {
                result = Settings.Default[binder.Name];

                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder != null && settingsMembers.Contains(binder.Name))
            {
                Settings.Default[binder.Name] = value;

                return true;
            }

            return base.TrySetMember(binder, value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return settingsMembers;
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
                    Error = (Settings.Default.Rows < 9 || Settings.Default.Rows > 24) ?
                        ResourceHelper.FindText("InvalidHeightMessage") : null;
                }
                else if ("Columns".Equals(propertyName, StringComparison.Ordinal))
                {
                    Error = (Settings.Default.Columns < 9 || Settings.Default.Columns > 30) ?
                        ResourceHelper.FindText("InvalidWidthMessage") : null;
                }
                else if ("Mines".Equals(propertyName, StringComparison.Ordinal))
                {
                    Error = (Settings.Default.Mines < 10 || Settings.Default.Mines > Settings.Default.Rows * Settings.Default.Columns) ?
                        ResourceHelper.FindText("InvalidMinesMessage") : null;
                }
                else if ("GameHistoryFolder".Equals(propertyName, StringComparison.Ordinal))
                {
                    Error = Directory.Exists(Settings.Default.GameHistoryFolder) ?
                        null : ResourceHelper.FindText("HistoryNotExistMessage");
                }
                else
                {
                    Error = null;
                }

                return Error;
            }
        }

        public IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.OptionCommandBindings; }
        }

        public void Commit()
        {
            Settings.Default.Save();
        }

        public void Rollback()
        {
            Settings.Default.Reload();
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            TriggerPropertyChanged(e.PropertyName);
        }

        public void OnLoaded(object sender) { /* Nothing */ }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool SetProperty<T>(ref T property, T newValue, string propertyName)
        {
            if (!Object.Equals(property, newValue))
            {
                property = newValue;
                TriggerPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        private void TriggerPropertyChanged(string propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
