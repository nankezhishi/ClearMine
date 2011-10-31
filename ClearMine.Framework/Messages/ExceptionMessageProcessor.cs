namespace ClearMine.Framework.Messages
{
    using System.Diagnostics;
    using System.Windows;
    using ClearMine.Framework.Dialogs;

    public class ExceptionMessageProcessor
    {
        public void HandleMessage(ExceptionMessage message)
        {
            if (message.Exception != null)
            {
                Trace.Write(message.Exception);
                message.HandlingResult = ExceptionBox.Show(message.Exception, Window.GetWindow(message.Source as DependencyObject ?? Application.Current.MainWindow));
            }
        }
    }
}
