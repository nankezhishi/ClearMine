namespace ClearMine.VM
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Modularity;

    /// <summary>
    /// 
    /// </summary>
    public class PluginsViewModel : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return new CommandBinding[0]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPlugin> Plugins
        {
            get { return ModularityManager.LoadedPlugins; }
        }
    }
}
