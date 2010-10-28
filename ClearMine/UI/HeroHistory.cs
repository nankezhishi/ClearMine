namespace ClearMine.UI
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("record")]
    public class HistoryRecord 
    {
        [XmlAttribute("score")]
        public int Score { get; set; }
        [XmlAttribute("when")]
        public DateTime Date { get; set; }
    }

    [Serializable]
    [XmlRoot("heroHistory")]
    public class HeroHistory
    {
        [XmlArray("records")]
        [XmlArrayItem("record")]
        public ObservableCollection<HistoryRecord> Items { get; set; }

        [XmlAttribute("level")]
        public Difficulty Level { get; set; }

        [XmlAttribute("played")]
        public int GamePlayed { get; set; }

        [XmlAttribute("won")]
        public int GameWon { get; set; }

        [XmlAttribute("longestWinning")]
        public int LongestWinning { get; set; }

        [XmlAttribute("longestLosing")]
        public int LogestLosing { get; set; }

        [XmlAttribute("current")]
        public int CurrentStatus { get; set; }

        [XmlAttribute("everage")]
        public int EverageScore { get; set; }
    }

    [XmlRoot("heroList")]
    public class HeroHistoryList
    {
        [XmlElement("heroOnLevel")]
        public ObservableCollection<HeroHistory> Heros { get; set; }
    }
}
