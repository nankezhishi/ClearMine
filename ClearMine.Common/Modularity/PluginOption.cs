namespace ClearMine.Common.Modularity
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("option")]
    public class PluginOption
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("type")]
        public string ValueType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Predicate<object> ValueValidator { get; set; }
    }
}
