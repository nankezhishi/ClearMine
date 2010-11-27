namespace ClearMine.VM.Commands
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Commands;
    using ClearMine.Localization;
    using ClearMine.UI.Dialogs;

    public static class GameCommandBindings
    {
        #region Show Statistics Command
        private static CommandBinding statisticsBinding = new CommandBinding(GameCommands.ShowStatistics,
            new ExecutedRoutedEventHandler(OnStatisticsExecuted), new CanExecuteRoutedEventHandler(OnStaisticsCanExecute));

        public static CommandBinding StatisticsBinding
        {
            get { return statisticsBinding; }
        }

        private static void OnStatisticsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow();
            statisticsWindow.Owner = Application.Current.MainWindow;
            statisticsWindow.DataContext = new StatisticsViewModel()
            {
                SelectedLevel = Settings.Default.Difficulty != Difficulty.Custom ? Settings.Default.Difficulty : Difficulty.Beginner
            };
            statisticsWindow.ShowDialog();
        }

        private static void OnStaisticsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region Close Command
        private static CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close,
            new ExecutedRoutedEventHandler(OnCloseExecuted), new CanExecuteRoutedEventHandler(OnCloseCanExecuted));

        public static CommandBinding CloseBinding
        {
            get { return closeBinding; }
        }

        private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private static void OnCloseCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region About Command
        private static CommandBinding aboutBinding = new CommandBinding(GameCommands.About,
            new ExecutedRoutedEventHandler(OnAboutExecuted), new CanExecuteRoutedEventHandler(OnAboutCanExecute));

        public static CommandBinding AboutBinding
        {
            get { return aboutBinding; }
        }

        private static void OnAboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var about = new AboutDialog();
            about.Owner = Application.Current.MainWindow;
            about.ShowDialog();
        }

        private static void OnAboutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region Feeback Command
        private static CommandBinding feedbackBinding = new CommandBinding(GameCommands.Feedback,
            new ExecutedRoutedEventHandler(OnFeedbackExecuted), new CanExecuteRoutedEventHandler(OnFeedbackCanExecute));

        public static CommandBinding FeedbackBinding
        {
            get { return feedbackBinding; }
        }

        private static void OnFeedbackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EmailHelper.Send("hi", LocalizationHelper.FindText("ClearMineFeedbackTitle"), "nankezhishi@hotmail.com");
        }

        private static void OnFeedbackCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region ShowLogCommand
        private static CommandBinding showLogBinding = new CommandBinding(GameCommands.ShowLog,
            new ExecutedRoutedEventHandler(OnShowLogExecuted), new CanExecuteRoutedEventHandler(OnShowLogCanExecute));

        public static CommandBinding ShowLogBinding
        {
            get { return showLogBinding; }
        }

        private static void OnShowLogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var outputWindow = new OutputWindow();
            outputWindow.Owner = Application.Current.MainWindow;
            outputWindow.Show();
        }

        private static void OnShowLogCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region Switch Language Command

        private static CommandBinding switchLanguageBinding = new CommandBinding(GameCommands.SwitchLanguage,
            new ExecutedRoutedEventHandler(OnSwitchLanguageExecuted), new CanExecuteRoutedEventHandler(OnSwitchLanguageCanExecute));

        public static CommandBinding SwitchLanguageBinding
        {
            get { return switchLanguageBinding; }
        }

        private static void OnSwitchLanguageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResourceDictionary languageDictionary = new ResourceDictionary()
            {
                Source = new Uri(String.Format(CultureInfo.InvariantCulture, "/ClearMine.Localization;component/{0}/Overall.xaml", e.Parameter.ToString()), UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries[0] = languageDictionary;
        }

        private static void OnSwitchLanguageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        } 

        #endregion
    }
}
