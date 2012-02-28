namespace ClearMine.Common.Model
{
    using System;
    using System.Xml.Serialization;

    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("record")]
    public class HistoryRecord
    {
        /// <summary>
        /// Gets or sets the time used in seconds.
        /// </summary>
        [XmlAttribute("score")]
        public double Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("when")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("shoot")]
        public string ScreenShoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Uri ScreenShootUri
        {
            get { return new Uri(Infrastructure.GetAbsolutePath(ScreenShoot)); }
        }
    }
}
