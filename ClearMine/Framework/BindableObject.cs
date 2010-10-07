namespace ClearMine.Framework
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using ClearMine.Utilities;

    internal class BindableObject : INotifyPropertyChanged
    {
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
                MethodBase caller = new StackTrace().GetFrame(1).GetMethod();
                if (!caller.IsPropertySetter())
                    throw new InvalidProgramException("SetProperty method without propertyName parameter can only be called from property setter.");

                string propertyName = caller.Name.Substring(4);
                SetProperty(ref property, newValue, propertyName);

                return true;
            }

            return false;
        }

        public bool SetProperty<T>(ref T property, T newValue, string propertyName)
        {
            if (!property.Equals(newValue))
            {
                property = newValue;
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }
    }
}
