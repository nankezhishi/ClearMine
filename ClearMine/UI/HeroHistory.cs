namespace ClearMine.UI
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
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

        [XmlIgnore]
        public int GameLost
        {
            get
            {
                Debug.Assert(GamePlayed >= GameWon);

                return GamePlayed - GameWon;
            }
        }

        [XmlIgnore]
        public double GameWonPercentage
        {
            get
            {
                if (GamePlayed == 0)
                {
                    return 0.0;
                }

                return GameWon / (double)GamePlayed;
            }
        }

        [XmlIgnore]
        public double GameLostPercentage
        {
            get
            {
                if (GamePlayed == 0)
                {
                    return 0.0;
                }

                return GameLost / (double)GamePlayed;
            }
        }

        public void IncreaseWon(int score, DateTime time)
        {
            EverageScore = (GameWon * EverageScore + score) / ++GameWon;
            ++GamePlayed;
            if (CurrentStatus >= 0)
            {
                ++CurrentStatus;
            }
            else
            {
                CurrentStatus = 1;
            }
            if (CurrentStatus > LongestWinning)
            {
                LongestWinning = CurrentStatus;
            }

            Items.Add(new HistoryRecord() { Score = score, Date = time });
        }

        public void IncreaseLost()
        {
            ++GamePlayed;
            if (CurrentStatus <= 0)
            {
                --CurrentStatus;
            }
            else
            {
                CurrentStatus = -1;
            }
            if (-CurrentStatus > LogestLosing)
            {
                LogestLosing = -CurrentStatus;
            }
        }

        public void Reset()
        {
            Items.Clear();
            GamePlayed = 0;
            GameWon = 0;
            LogestLosing = 0;
            LongestWinning = 0;
            CurrentStatus = 0;
            EverageScore = 0;
        }
    }

    [XmlRoot("heroList")]
    public class HeroHistoryList
    {
        [XmlElement("heroOnLevel")]
        public ObservableCollection<HeroHistory> Heros { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public HeroHistory GetByLevel(Difficulty level)
        {
            return Heros.FirstOrDefault(history => history.Level == level);
        }
    }
}
