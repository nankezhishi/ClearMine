namespace ClearMine.Common.ComponentModel.UI
{
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// 
    /// </summary>
    public abstract class ViewModelBase : BindableObject, IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract IEnumerable<CommandBinding> CommandBindings { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public virtual void OnLoaded(object sender) { }
    }
}
