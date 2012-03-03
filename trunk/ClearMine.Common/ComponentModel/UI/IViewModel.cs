namespace ClearMine.Common.ComponentModel.UI
{
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// 
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<CommandBinding> CommandBindings { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        void OnLoaded(object sender);
    }
}
