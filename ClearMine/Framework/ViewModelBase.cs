namespace ClearMine.Framework
{
    internal class ViewModelBase : BindableObject
    {
        private bool loaded;
        private bool initialized;

        public bool Loaded
        {
            get { return loaded; }
            set { SetProperty(ref loaded, value, "Loaded"); }
        }

        public bool Initialized
        {
            get { return initialized; }
            set { SetProperty(ref initialized, value, "Initialized"); }
        }
    }
}
