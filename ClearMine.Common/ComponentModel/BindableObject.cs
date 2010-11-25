namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("Microsoft.Design", "CA1045",
            Justification = "Use ref to change the value of a field in a sub class easily.")]
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
