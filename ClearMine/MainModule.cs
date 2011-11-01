namespace ClearMine
{
    using System.Windows;
    using System.Windows.Media;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Framework.Messages;
    using ClearMine.VM;

    internal class MainModule : IModule
    {
        public void InitializeModule()
        {
            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                var message = new ExceptionMessage(e.Exception);
                MessageManager.GetMessageAggregator<ExceptionMessage>().SendMessage(message);
                e.Handled = (bool)message.HandlingResult;
            };

            Application.Current.Startup += new StartupEventHandler(OnApplicationStartup);
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(OnApplicationStartup);

            var mainWindow = new Window()
            {
                Width = 480,
                Height = 480,
                UseLayoutRounding = true,
                Background = Brushes.Silver,
                DataContext = new ClearMineViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TextOptions.SetTextFormattingMode(mainWindow, TextFormattingMode.Display);
            mainWindow.SetResourceReference(Window.TitleProperty, "ApplicationTitle");
            mainWindow.SetBinding(Window.ContentProperty, ".");
            mainWindow.Show();
        }
    }
}
