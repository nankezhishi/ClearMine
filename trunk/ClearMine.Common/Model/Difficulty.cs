namespace ClearMine.Common.Model
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public enum Difficulty
    {
        [XmlEnum("beginner")]
        Beginner,
        [XmlEnum("interMediate")]
        Intermediate,
        [XmlEnum("advanced")]
        Advanced,
        [XmlEnum("custom")]
        Custom
    }
}
