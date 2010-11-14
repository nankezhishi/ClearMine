namespace ClearMine.Common.ComponentModel
{
    using System.Windows.Input;
    using System.Collections.Generic;

    internal abstract class ViewModelBase : BindableObject
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

        public abstract IEnumerable<CommandBinding> GetCommandBindings();
    }
}
