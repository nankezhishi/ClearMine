namespace ClearMine.VM.Commands
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;

    using ClearMine.Common;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Behaviors;
    using ClearMine.Framework.Commands;
    using ClearMine.Framework.Dialogs;
    using ClearMine.Framework.Interactivity;
    using ClearMine.Localization;
    using Microsoft.Win32;

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
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.StatisticsWindow, ClearMine.Dialogs"),
                Data =  new StatisticsViewModel()
                {
                    SelectedLevel = Settings.Default.Difficulty != Difficulty.Custom ? Settings.Default.Difficulty : Difficulty.Beginner
                }
            });
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
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.AboutDialog, ClearMine.Dialogs")
            });
        }

        private static void OnAboutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        private static CommandBinding viewHelpBinding = new CommandBinding(ApplicationCommands.Help,
            new ExecutedRoutedEventHandler(OnHelpExecuted), new CanExecuteRoutedEventHandler(OnHelpCanExecute));

        public static CommandBinding ViewHelpBinding
        {
            get { return viewHelpBinding; }
        }

        private static void OnHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var helpName = Settings.Default.HelpDocumentName;

            if (!String.IsNullOrWhiteSpace(helpName) && helpName.EndsWith("chm"))
            {
                try
                {
                    Process.Start(helpName);
                }
                catch (FileNotFoundException)
                {
                    Trace.TraceError(LocalizationHelper.FindText("CannotFindHelpFile", helpName));
                }
            }
            else
            {
                Trace.TraceError(LocalizationHelper.FindText("InvalidHelpFileType", helpName));
            }
        }

        private static void OnHelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        } 

        #region Feeback Command
        private static CommandBinding feedbackBinding = new CommandBinding(GameCommands.Feedback,
            new ExecutedRoutedEventHandler(OnFeedbackExecuted), new CanExecuteRoutedEventHandler(OnFeedbackCanExecute));

        public static CommandBinding FeedbackBinding
        {
            get { return feedbackBinding; }
        }

        private static void OnFeedbackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EmailHelper.Send(LocalizationHelper.FindText("ClearMineFeedbackContent"),
                LocalizationHelper.FindText("ClearMineFeedbackTitle"), Settings.Default.FeedBackEmail);
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
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.OutputWindow, ClearMine.Dialogs")
            });
        }

        private static void OnShowLogCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region Switch Language Command

        private static CommandBinding switchLanguageBinding = new CommandBinding(GameCommands.SwitchLanguage,
            new ExecutedRoutedEventHandler(OnSwitchLanguageExecuted),
            new CanExecuteRoutedEventHandler(OnSwitchLanguageCanExecute));

        public static CommandBinding SwitchLanguageBinding
        {
            get { return switchLanguageBinding; }
        }

        private static void OnSwitchLanguageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var path = String.Empty;

            if (GameCommands.CustomLanguageKey.Equals(e.Parameter.ToString(), StringComparison.Ordinal))
            {
                var openFileDialog = new OpenFileDialog()
                {
                    DefaultExt = ".xaml",
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = LocalizationHelper.FindText("LanguageFileFilter"),
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    path = openFileDialog.FileName;
                }
                else
                {
                    Interaction.FindBehavior<AutoCheckMenuItemsBehavior>(e.OriginalSource as DependencyObject, b => b.UndoMenuItemCheck());
                    return;
                }
            }
            else
            {
                path = String.Format(CultureInfo.InvariantCulture,
                    "/ClearMine.Localization;component/{0}/Overall.xaml", e.Parameter.ToString());
            }

            try
            {
                ResourceDictionary languageDictionary = new ResourceDictionary()
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                if (VerifyLanguageResourceFile(Application.Current.Resources.MergedDictionaries[0], languageDictionary))
                {
                    Application.Current.Resources.MergedDictionaries[0] = languageDictionary;
                }
            }
            catch (XamlParseException ex)
            {
                var message = String.Format(CultureInfo.InvariantCulture, 
                    LocalizationHelper.FindText("LanguageResourceParseError"), ex.Message);
                Interaction.FindBehavior<AutoCheckMenuItemsBehavior>(e.OriginalSource as DependencyObject, b => b.UndoMenuItemCheck());
                MessageBox.Show(message, LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void OnSwitchLanguageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        private static bool VerifyLanguageResourceFile(ResourceDictionary existing, ResourceDictionary newResource)
        {
            foreach (var resource in newResource.Values)
            {
                if (!(resource is string))
                {
                    MessageBox.Show(LocalizationHelper.FindText("InvalidLanguageResourceType"),
                        LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            var newKeys = newResource.Keys.Cast<string>();

            foreach (var existingKey in existing.Keys)
            {
                if (newKeys.Contains(existingKey))
                {
                    continue;
                }
                else
                {
                    string message = String.Format(CultureInfo.InvariantCulture,
                        LocalizationHelper.FindText("MissingLanguageResourceKey"), existingKey);
                    MessageBox.Show(message, LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
