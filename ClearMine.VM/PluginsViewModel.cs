namespace ClearMine.VM
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.VM.Commands;

    /// <summary>
    /// 
    /// </summary>
    public class PluginsViewModel : ViewModelBase, ITransaction
    {
        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.OptionCommandBindings; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPlugin> Plugins
        {
            get { return ModularityManager.LoadedPlugins; }
        }

        public void Commit()
        {
            Settings.Default.Save();
        }

        public void Rollback()
        {
            Settings.Default.Reload();
        }
    }
}
