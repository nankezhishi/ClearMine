namespace ClearMine.UI.Dialogs
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Properties;

    internal class StatisticsViewModel : ViewModelBase
    {
        private Difficulty selectedLevel;

        #region Reset Command
        private static ICommand reset = new RoutedUICommand("Reset", "Reset", typeof(StatisticsViewModel));
        private static CommandBinding resetBinding = new CommandBinding(Reset,
            new ExecutedRoutedEventHandler(OnResetExecuted), new CanExecuteRoutedEventHandler(OnResetCanExecute));
        public static ICommand Reset
        {
            get { return reset; }
        }

        public static CommandBinding ResetBinding
        {
            get { return resetBinding; }
        }

        private static void OnResetExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to reset all your statistics to zero?", "ClearMine - Reset Statistics",
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
            set { SetProperty(ref selectedLevel, value); }
        }
    }
}
