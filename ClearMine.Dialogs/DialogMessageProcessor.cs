namespace ClearMine.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using ClearMine.Framework.Messages;

    /// <summary>
    /// 
    /// </summary>
    public class DialogMessageProcessor
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

        public void HandleMessage(ShowDialogMessage message)
        {
            if (message == null)
                return;

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
