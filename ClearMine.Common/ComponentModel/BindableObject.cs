namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.ComponentModel;

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
