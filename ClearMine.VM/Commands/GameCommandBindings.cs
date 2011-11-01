namespace ClearMine.VM.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Threading;

    using ClearMine.Common;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Behaviors;
    using ClearMine.Framework.Commands;
    using ClearMine.Framework.Interactivity;
    using ClearMine.Framework.Messages;
    using ClearMine.GameDefinition;
    using Microsoft.Win32;
    using DialogResult = System.Windows.Forms.DialogResult;
    using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

    public static class GameCommandBindings
    {
        #region Option Command
        private static ICommand option = new RoutedUICommand("Option", "Option",
            typeof(ClearMineViewModel), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control) });
        private static CommandBinding optionBinding = new CommandBinding(Option, OnOptionExecuted);
        public static ICommand Option
        {
            get { return option; }
        }

        private static void OnOptionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = e.ExtractDataContext<ClearMineViewModel>();
            bool shouldResume = false;
            if (viewModel.Game.GameState == GameState.Started)
            {
                shouldResume = true;

                // Pause the game may takes long time. Needn't wait that finish; 
                Application.Current.Dispatcher.BeginInvoke(new Action(viewModel.Game.PauseGame), DispatcherPriority.Background);
            }

            var message = new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.OptionsDialog, ClearMine.Dialogs", true),
                Data = new OptionsViewModel(),
            };

            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(message);

            if (message.HandlingResult != null && ((bool?)message.HandlingResult).Value)
            {
                viewModel.StartNewGame();
            }
            else if (shouldResume)
            {
                viewModel.Game.ResumeGame();
            }
        }
        #endregion
        #region SaveAsCommand
        private static CommandBinding saveAsBinding = new CommandBinding(ApplicationCommands.SaveAs, OnSaveAsExecuted, OnSaveAsCanExecute);

        private static void OnSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>().SaveCurrentGame();
        }

        private static void OnSaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.ExtractDataContext<ClearMineViewModel>().Game.GameState == GameState.Started;
        }
        #endregion
        #region OpenCommand
        private static CommandBinding openBinding = new CommandBinding(ApplicationCommands.Open, OnOpenExecuted);

        private static void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = Settings.Default.SavedGameExt;
            openFileDialog.Filter = LocalizationHelper.FindText("SavedGameFilter", Settings.Default.SavedGameExt);
            if (openFileDialog.ShowDialog() == true)
            {
                e.ExtractDataContext<ClearMineViewModel>().LoadSavedGame(openFileDialog.FileName);
            }
        }
        #endregion
        #region NewGame Command
        private static CommandBinding newGameBinding = new CommandBinding(ApplicationCommands.New, OnNewGameExecuted);

        private static void OnNewGameExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>().StartNewGame();
        }
        #endregion
        #region Refresh Binding
        private static CommandBinding refreshBinding = new CommandBinding(NavigationCommands.Refresh, OnRefreshExecuted);

        private static void OnRefreshExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ClearMineViewModel>().Game.Restart();
        }
        #endregion

        #region Show Statistics Command
        private static CommandBinding statisticsBinding = new CommandBinding(GameCommands.ShowStatistics, OnStatisticsExecuted);

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
        #endregion

        #region Close Command
        private static CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close, OnCloseExecuted);

        private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        #endregion

        #region About Command
        private static CommandBinding aboutBinding = new CommandBinding(GameCommands.About, OnAboutExecuted);

        private static void OnAboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.AboutDialog, ClearMine.Dialogs")
            });
        }
        #endregion

        private static CommandBinding viewHelpBinding = new CommandBinding(ApplicationCommands.Help, OnHelpExecuted);

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

        #region Feeback Command
        private static CommandBinding feedbackBinding = new CommandBinding(GameCommands.Feedback, OnFeedbackExecuted);

        private static void OnFeedbackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EmailHelper.Send(LocalizationHelper.FindText("ClearMineFeedbackContent"),
                LocalizationHelper.FindText("ClearMineFeedbackTitle"), Settings.Default.FeedBackEmail);
        }
        #endregion

        #region ShowLogCommand
        private static CommandBinding showLogBinding = new CommandBinding(GameCommands.ShowLog, OnShowLogExecuted);

        private static void OnShowLogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.GetMessageAggregator<ShowDialogMessage>().SendMessage(new ShowDialogMessage()
            {
                Source = e.OriginalSource,
                DialogType = Type.GetType("ClearMine.UI.Dialogs.OutputWindow, ClearMine.Dialogs"),
                ModuleDialog = false
            });
        }
        #endregion

        #region Switch Language Command
        private static CommandBinding switchLanguageBinding = new CommandBinding(GameCommands.SwitchLanguage, OnSwitchLanguageExecuted);

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
        #endregion

        #region Close Command
        private static CommandBinding optionCloseBinding = new CommandBinding(ApplicationCommands.Close, OnOptionCloseExecuted);

        private static void OnOptionCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<OptionsViewModel>(vm =>
            {
                vm.Cancel();
                var window = Window.GetWindow(sender as DependencyObject);
                if (window != null)
                {
                    window.DialogResult = false;
                    window.Close();
                }
            });
        }
        #endregion

        #region Save Command
        private static CommandBinding saveBinding = new CommandBinding(ApplicationCommands.Save, OnSaveExecuted, OnSaveCanExecuted);

        private static void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<OptionsViewModel>(vm =>
            {
                vm.Save();
                var window = Window.GetWindow(sender as DependencyObject);
                if (window != null)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            });
        }

        private static void OnSaveCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = String.IsNullOrWhiteSpace(e.ExtractDataContext<OptionsViewModel>().Error);
        }
        #endregion

        #region BrowseHistory Command
        private static CommandBinding browseHistoryBinding = new CommandBinding(OptionsCommands.BrowseHistory, OnBrowseHistoryExecuted, OnBrowseHistoryCanExecute);

        private static void OnBrowseHistoryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.Description = LocalizationHelper.FindText("BrowseGameFolderMessage");
            folderBrowser.SelectedPath = Path.GetFullPath(Settings.Default.GameHistoryFolder);
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.GameHistoryFolder = folderBrowser.SelectedPath;
            }
        }

        private static void OnBrowseHistoryCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Settings.Default.SaveGame;
        }
        #endregion

        #region Reset Command

        private static CommandBinding resetBinding = new CommandBinding(StatisticsCommands.Reset, OnResetExecuted);

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
        #endregion

        public static IEnumerable<CommandBinding> GetGameWonCommandBindings()
        {
            yield return statisticsBinding;
        }

        public static IEnumerable<CommandBinding> GetStatisticsCommandBindings()
        {
            yield return resetBinding;
        }

        public static IEnumerable<CommandBinding> GetOptionCommandBindings()
        {
            yield return browseHistoryBinding;
            yield return optionCloseBinding;
            yield return saveBinding;
        }

        public static IEnumerable<CommandBinding> GetGameCommandBindings()
        {
            // Arrange in alphabetical order.
            yield return aboutBinding;
            yield return closeBinding;
            yield return feedbackBinding;
            yield return newGameBinding;
            yield return openBinding;
            yield return optionBinding;
            yield return refreshBinding;
            yield return saveAsBinding;
            yield return showLogBinding;
            yield return statisticsBinding;
            yield return switchLanguageBinding;
            yield return viewHelpBinding;
        }

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
                if (!newKeys.Contains(existingKey))
                {
                    var message = String.Format(CultureInfo.InvariantCulture,
                        LocalizationHelper.FindText("MissingLanguageResourceKey"), existingKey);
                    MessageBox.Show(message, LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
