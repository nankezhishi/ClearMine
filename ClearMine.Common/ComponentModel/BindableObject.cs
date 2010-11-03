namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using ClearMine.Common.Utilities;

    public class BindableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public bool SetProperty<T>(ref T property, T newValue)
        {
            if (!property.Equals(newValue))
            {
                // This takes more CPU and Memory.
                MethodBase caller = new StackTrace().GetFrame(1).GetMethod();
                if (!caller.IsPropertySetter())
                    throw new InvalidProgramException("This method can only be called from property setter.");

                string propertyName = caller.Name.Substring(4);
                SetProperty(ref property, newValue, propertyName);

                return true;
            }

            return false;
        }

        public bool SetProperty<T>(ref T property, T newValue, string propertyName)
        {
            if (!Object.Equals(property, newValue))
            {
                property = newValue;
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }
    }
}
