namespace ClearMine.Common.ComponentModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    internal abstract class ViewModelBase : BindableObject
    {
        public abstract IEnumerable<CommandBinding> GetCommandBindings();

        public virtual void OnLoaded(object sender) { }
    }
}
