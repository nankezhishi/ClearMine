namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using ClearMine.Common.Modularity;
    using System.Collections.ObjectModel;

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

    /// <summary>
    /// .NET doesn't support serializing a dictionary.
    /// So I create this class to persist data map. This could be used in Plugin Options.
    /// </summary>
    [Serializable]
    [XmlRoot("map")]
    public class DataMap
    {
        /// <summary>
        /// 
        /// </summary>
        public DataMap()
        {
            Items = new Collection<DataMapItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public Collection<DataMapItem> Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                var mapItem = Items.FirstOrDefault(item => String.Compare(item.Key, key) == 0);

                if (mapItem == null)
                {
                    return null;
                }
                else
                {
                    return mapItem.Value;
                }
            }
            set
            {
                var mapItem = Items.FirstOrDefault(item => String.Compare(item.Key, key) == 0);
                if (mapItem == null)
                {
                    Items.Add(new DataMapItem() { Key = key, Value = value });
                }
                else
                {
                    mapItem.Value = value;
                }
            }
        }
    }
}
