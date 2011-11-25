namespace ClearMine.Common.Modularity
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class PluginOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ValueType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Predicate<object> ValueValidator { get; set; }
    }
}
