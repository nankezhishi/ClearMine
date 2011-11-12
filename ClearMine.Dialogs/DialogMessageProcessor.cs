namespace ClearMine.Dialogs
{
    using System;
    using System.Windows;
    using ClearMine.Framework.Messages;

    public class DialogMessageProcessor
    {
        public void HandleMessage(ShowDialogMessage message)
        {
            if (message == null)
                return;

            var window = Activator.CreateInstance(message.DialogType) as Window;
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
