namespace ClearMine.Common.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Serialization;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Utilities;

    [Serializable]
    [XmlRoot("heroHistory")]
    public class HeroHistory : BindableObject
    {
        [XmlArray("records")]
        [XmlArrayItem("record")]
        public ObservableCollection<HistoryRecord> Items { get; set; }

        [XmlAttribute("level")]
        public Difficulty Level { get; set; }

        [XmlIgnore]
        public string LevelForDisplay
        {
            get { return ResourceHelper.FindText(Level.ToString()); }
        }

        [XmlAttribute("played")]
        public int GamePlayed { get; set; }

        [XmlAttribute("won")]
        public int GameWon { get; set; }

        [XmlAttribute("lost")]
        public int GameLost { get; set; }

        [XmlAttribute("longestWinning")]
        public int LongestWinning { get; set; }

        [XmlAttribute("longestLosing")]
        public int LongestLosing { get; set; }

        [XmlAttribute("current")]
        public int CurrentStatus { get; set; }

        [XmlAttribute("average")]
        public double AverageScore { get; set; }

        [XmlIgnore]
        public int GameUndone
        {
            get
            {
                Debug.Assert(GamePlayed >= GameWon + GameLost);

                return GamePlayed - GameWon - GameLost;
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

        public void IncreaseWon(double score, DateTime time, string screenShoot)
        {
            AverageScore = (GameWon * AverageScore + score) / ++GameWon;
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

            Items.Add(new HistoryRecord()
            {
                Score = score, Date = time, ScreenShoot = screenShoot
            });
        }

        public void IncreaseUndone()
        {
            ++GamePlayed;
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

            if (-CurrentStatus > LongestLosing)
            {
                LongestLosing = -CurrentStatus;
            }
        }

        public void Reset()
        {
            Items.Clear();
            GamePlayed = 0;
            GameWon = 0;
            GameLost = 0;
            LongestLosing = 0;
            LongestWinning = 0;
            CurrentStatus = 0;
            AverageScore = 0;

            // Update UI after reset.
            TriggerPropertyChanged("Items");
            TriggerPropertyChanged("GamePlayed");
            TriggerPropertyChanged("GameWon");
            TriggerPropertyChanged("GameLost");
            TriggerPropertyChanged("LogestLosing");
            TriggerPropertyChanged("LongestWinning");
            TriggerPropertyChanged("CurrentStatus");
            TriggerPropertyChanged("EverageScore");
            TriggerPropertyChanged("GameWonPercentage");
            TriggerPropertyChanged("GameLostPercentage");
        }
    }
}
