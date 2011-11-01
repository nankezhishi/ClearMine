namespace ClearMine.VM
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.VM.Commands;

    internal class StatisticsViewModel : ViewModelBase
    {
        private static string defaultSortColumn;
        private Difficulty selectedLevel;

        static StatisticsViewModel()
        {
            defaultSortColumn = GenericExtension.GetMemberName<HistoryRecord>(r => r.Score);
        }

        public IEnumerable<HeroHistory> HistoryList
        {
            get { return Settings.Default.HeroList.Heroes; }
        }

        public Difficulty SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                SetProperty(ref selectedLevel, value, "SelectedLevel");

                // Sort the history by score by default.
                var histories = Settings.Default.HeroList.GetByLevel(value).Items;
                var view = CollectionViewSource.GetDefaultView(histories);
                if (view != null && view.SortDescriptions.Count == 0)
                {
                    view.SortDescriptions.Add(new SortDescription(defaultSortColumn, ListSortDirection.Ascending));
                }
            }
        }

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            return GameCommandBindings.GetStatisticsCommandBindings();
        }
    }
}
