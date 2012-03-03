namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using ClearMine.Common.Modularity;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(List<PluginOption>))]
    public class DataMapItem
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("value")]
        public object Value { get; set; }
    }
}
