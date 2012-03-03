namespace ClearMine.Common.Modularity
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    using ClearMine.Common.Utilities;

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
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        [ReadOnly(true)]
        public string Name
        {
            get { return ResourceHelper.FindText(NameKey); }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("name")]
        public string NameKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        [ReadOnly(true)]
        public string Description { get { return ResourceHelper.FindText(DescriptionKey); } }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("description")]
        public string DescriptionKey { get; set; }

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
