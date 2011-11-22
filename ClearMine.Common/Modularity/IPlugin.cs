namespace ClearMine.Common.Modularity
{
    /// <summary>
    /// A plugin refer to a function or feature makes the game runs better. No required to run.
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
        void Initialize();
    }
}
