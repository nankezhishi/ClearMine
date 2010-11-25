namespace ClearMine.Common.ComponentModel
{
    using System.Windows.Input;
    using System.Collections.Generic;

    internal abstract class ViewModelBase : BindableObject
    {
        public abstract IEnumerable<CommandBinding> GetCommandBindings();
    }
}
