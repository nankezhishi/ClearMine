namespace ClearMine.VM.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Model;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework;
    using ClearMine.Framework.Messages;
    using ClearMine.GameDefinition;
    using Microsoft.Win32;
    using DialogResult = System.Windows.Forms.DialogResult;
    using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

    public static class GameCommandBindings
    {
        #region Option Command
        private static CommandBinding optionBinding = new CommandBinding(GameCommands.Option, OnOptionExecuted);

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

            var result = MessageManager.SendMessage<ShowDialogMessage>(e.OriginalSource, PopupDialog.Options, new OptionsViewModel());

            if ((bool)result)
            {
                viewModel.StartNewGame();
            }
            else if (shouldResume)
            {
                viewModel.Game.ResumeGame();
            }
        }
        #endregion
        #region Plugins Command
        private static CommandBinding pluginsBinding = new CommandBinding(GameCommands.Plugins, OnPluginsExecuted, OnPluginsCanExecuted);

        private static void OnPluginsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.SendMessage<ShowDialogMessage>(e.OriginalSource, PopupDialog.Plugins, new PluginsViewModel());
        }

        private static void OnPluginsCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModularityManager.LoadedPlugins.Count() > 0;
        }
        #endregion
        #region SaveAsCommand
        private static CommandBinding saveAsBinding = new CommandBinding(ApplicationCommands.SaveAs, OnSaveAsExecuted, OnSaveAsCanExecute);

        private static void OnSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Infrastructure.Container.GetExportedValue<IGameSerializer>().SaveGame(e.ExtractDataContext<ClearMineViewModel>().Game);
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
            openFileDialog.Filter = ResourceHelper.FindText("SavedGameFilter", Settings.Default.SavedGameExt);
            if (openFileDialog.ShowDialog() == true)
            {
                Infrastructure.Container.GetExportedValue<IGameSerializer>().LoadGame(openFileDialog.FileName,
                    e.ExtractDataContext<ClearMineViewModel>().Game.GetType());
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
            MessageManager.SendMessage<ShowDialogMessage>(e.OriginalSource, PopupDialog.Statistics,
                new StatisticsViewModel() {
                    SelectedLevel = Settings.Default.Difficulty != Difficulty.Custom ? Settings.Default.Difficulty : Difficulty.Beginner
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
            MessageManager.SendMessage<ShowDialogMessage>(e.OriginalSource, PopupDialog.About);
        }
        #endregion
        #region ViewHelp Binding
        private static CommandBinding viewHelpBinding = new CommandBinding(ApplicationCommands.Help, OnHelpExecuted);

        private static void OnHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.SendMessage<HelpRequestedMessage>();
        } 
        #endregion
        #region Feeback Command
        private static CommandBinding feedbackBinding = new CommandBinding(GameCommands.Feedback, OnFeedbackExecuted);

        private static void OnFeedbackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WebTools.SendEmail(ResourceHelper.FindText("ClearMineFeedbackContent"),
                ResourceHelper.FindText("ClearMineFeedbackTitle"), Settings.Default.FeedBackEmail);
        }
        #endregion
        #region ShowLogCommand
        private static CommandBinding showLogBinding = new CommandBinding(GameCommands.ShowLog, OnShowLogExecuted);

        private static void OnShowLogExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.SendMessage<ShowDialogMessage>(e.OriginalSource, PopupDialog.Output, false);
        }
        #endregion
        #region Switch Language Command
        private static CommandBinding switchLanguageBinding = new CommandBinding(GameCommands.SwitchLanguage, OnSwitchLanguageExecuted);

        private static void OnSwitchLanguageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.SendMessage<SwitchLanguageMessage>(e.Parameter, e.OriginalSource);
        }
        #endregion
        #region Switch Theme Command
        private static CommandBinding switchThemeBinding = new CommandBinding(GameCommands.SwitchTheme, OnSwitchThemeExecuted);

        private static void OnSwitchThemeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageManager.SendMessage<SwitchThemeMessage>(e.Parameter, e.OriginalSource);
        }
        #endregion
        #region Close Command
        private static CommandBinding optionCloseBinding = new CommandBinding(ApplicationCommands.Close, OnOptionCloseExecuted);

        private static void OnOptionCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ITransaction>(vm =>
            {
                vm.Rollback();
                (sender as DependencyObject).CloseParentWindow(false);
            });
        }
        #endregion
        #region Maximize Command
        private static CommandBinding maximizeBinding = new CommandBinding(UICommands.Maximize, OnMaximizeExecuted);

        private static void OnMaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            else if (Application.Current.MainWindow.WindowState == WindowState.Normal)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        #endregion
        #region Minimize Command
        private static CommandBinding minimizeBinding = new CommandBinding(UICommands.Minimize, OnMinimizeExecuted);

        private static void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        #endregion
        #region Save Command
        private static CommandBinding saveSettingsBinding = new CommandBinding(ApplicationCommands.Save, OnSaveSettingsExecuted, OnSaveSettingsCanExecuted);

        private static void OnSaveSettingsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.ExtractDataContext<ITransaction>(vm =>
            {
                vm.Commit();
                (sender as DependencyObject).CloseParentWindow(true);
            });
        }

        private static void OnSaveSettingsCanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = String.IsNullOrWhiteSpace(e.ExtractDataContext<IDataErrorInfo>().Error);
        }
        #endregion
        #region BrowseHistory Command
        private static CommandBinding browseHistoryBinding = new CommandBinding(GameCommands.BrowseHistory, OnBrowseHistoryExecuted, OnBrowseHistoryCanExecute);

        private static void OnBrowseHistoryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.ShowNewFolderButton = true;
                folderBrowser.Description = ResourceHelper.FindText("BrowseGameFolderMessage");
                folderBrowser.SelectedPath = Path.GetFullPath(Settings.Default.GameHistoryFolder);
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.GameHistoryFolder = folderBrowser.SelectedPath;
                }
            }
        }

        private static void OnBrowseHistoryCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Settings.Default.SaveGame;
        }
        #endregion
        #region Reset Command

        private static CommandBinding resetBinding = new CommandBinding(GameCommands.Reset, OnResetExecuted);

        private static void OnResetExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(ResourceHelper.FindText("ResetHistoryMessage"), ResourceHelper.FindText("ResetHistoryTitle"),
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

        public static IEnumerable<CommandBinding> GameWonCommandBindings
        {
            get { return new[] { statisticsBinding }; }
        }

        public static IEnumerable<CommandBinding> StatisticsCommandBindings
        {
            get { return new[] { resetBinding }; }
        }

        public static IEnumerable<CommandBinding> OptionCommandBindings
        {
            get { return new[] { browseHistoryBinding, optionCloseBinding, saveSettingsBinding }; }
        }

        public static IEnumerable<CommandBinding> MainCommandBindings
        {
            get
            {
                // Arrange in alphabetical order.
                return new[]
                {
                    aboutBinding,
                    closeBinding,
                    feedbackBinding,
                    maximizeBinding,
                    minimizeBinding,
                    newGameBinding,
                    openBinding,
                    optionBinding,
                    pluginsBinding,
                    refreshBinding,
                    saveAsBinding,
                    showLogBinding,
                    statisticsBinding,
                    switchLanguageBinding,
                    switchThemeBinding,
                    viewHelpBinding,
                };
            }
        }
    }
}
