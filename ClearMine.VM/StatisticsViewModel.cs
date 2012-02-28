namespace ClearMine.VM
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Data;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Model;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.VM.Commands;

    public class StatisticsViewModel : ViewModelBase
    {
        private static string defaultSortColumn;
        private Difficulty selectedLevel;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static StatisticsViewModel()
        {
            defaultSortColumn = GenericExtension.GetMemberName<HistoryRecord>(r => r.Score);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
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

        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.StatisticsCommandBindings; }
        }
    }
}
