namespace ClearMine.Dialogs
{
    using System;
    using System.Windows;
    using ClearMine.Framework.Dialogs;

    public class DialogMessageProcessor
    {
        public void HandleMessage(ShowDialogMessage message)
        {
            var window = Activator.CreateInstance(message.DialogType) as Window;
            window.Owner = Window.GetWindow(message.Source as DependencyObject ?? Application.Current.MainWindow);
            if (message.Data != null)
            {
                window.DataContext = message.Data;
            }
            message.HandlingResult = window.ShowDialog();
        }
    }
}
