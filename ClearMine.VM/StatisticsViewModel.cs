namespace ClearMine.VM
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Localization;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Commands;

    internal class StatisticsViewModel : ViewModelBase
    {
        private static string defaultSortColumn;
        private Difficulty selectedLevel;

        static StatisticsViewModel()
        {
            defaultSortColumn = GenericExtension.GetMemberName<HistoryRecord>(r => r.Score);
        }

        #region Reset Command

        private static CommandBinding resetBinding = new CommandBinding(StatisticsCommands.Reset,
            new ExecutedRoutedEventHandler(OnResetExecuted), new CanExecuteRoutedEventHandler(OnResetCanExecute));

        public static CommandBinding ResetBinding
        {
            get { return resetBinding; }
        }

        private static void OnResetExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(LocalizationHelper.FindText("ResetHistoryMessage"), LocalizationHelper.FindText("ResetHistoryTitle"),
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                foreach (HeroHistory history in e.Parameter as IEnumerable)
                {
                    history.Reset();
                }

                Settings.Default.Save();
            }
        }

        private static void OnResetCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

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
            yield return ResetBinding;
        }
    }
}
