namespace ClearMine.Common.Model
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot("heroList")]
    public class HeroHistoryList
    {
        [XmlElement("heroOnLevel")]
        public ObservableCollection<HeroHistory> Heroes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public HeroHistory GetByLevel(Difficulty level)
        {
            return Heroes.FirstOrDefault(history => history.Level == level);
        }
    }
}
