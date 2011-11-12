namespace ClearMine.Common.ComponentModel
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public abstract class ViewModelBase : BindableObject
    {
        public abstract IEnumerable<CommandBinding> CommandBindings { get; }

        public virtual void OnLoaded(object sender) { }
    }
}
