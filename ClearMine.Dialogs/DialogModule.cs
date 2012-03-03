namespace ClearMine.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Framework.Messages;

    /// <summary>
    /// 
    /// </summary>
    public class DialogModule : IModule
    {
        private static Dictionary<PopupDialog, Type> dialogMapping = new Dictionary<PopupDialog, Type>()
        {
            { PopupDialog.About, typeof(AboutDialog) },
            { PopupDialog.ConfirmNewGame, typeof(ConfirmNewGameWindow) },
            { PopupDialog.GameLost, typeof(GameLostWindow) },
            { PopupDialog.GameWon, typeof(GameWonWindow) },
            { PopupDialog.Help, typeof(HelpDialog) },
            { PopupDialog.Options, typeof(OptionsDialog) },
            { PopupDialog.Output, typeof(OutputWindow) },
            { PopupDialog.Plugins, typeof(PluginsDialog) },
            { PopupDialog.Statistics, typeof(StatisticsWindow) }
        };

        public void InitializeModule()
        {
            MessageManager.SubscribeMessage<ShowDialogMessage>(HandleMessage);
        }

        public static void HandleMessage(ShowDialogMessage message)
        {
            if (message == null)
                return;

            if (!dialogMapping.ContainsKey(message.DialogType))
            {
                Trace.TraceError(String.Format("Dialog type {0} is not registed.", message.DialogType));
                return;
            }

            var window = Activator.CreateInstance(dialogMapping[message.DialogType]) as Window;
            window.Owner = Window.GetWindow(message.Source as DependencyObject ?? Application.Current.MainWindow);
            if (message.Data != null)
            {
                window.DataContext = message.Data;
            }
            if (message.ModuleDialog)
            {
                message.HandlingResult = window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }
    }
}
