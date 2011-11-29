namespace ClearMine.Common.Modularity
{
    using System.Collections.Generic;

    /// <summary>
    /// A plugin refer to a function or feature makes the game runs better. No required to run.
    /// I know .NET Provice MEF and MAF which probably do the same. But I want to make it simpler and do it myself.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<PluginOption> Options { get; }

        /// <summary>
        /// 
        /// </summary>
        void Initialize();
    }
}
