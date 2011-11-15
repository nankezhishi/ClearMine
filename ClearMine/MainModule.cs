namespace ClearMine
{
    using System.Windows;

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
                e.Handled = (bool)MessageManager.SendMessage<ExceptionMessage>(e.Exception);
            };

            Application.Current.Startup += new StartupEventHandler(OnApplicationStartup);
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(OnApplicationStartup);

            new Window()
            {
                Width = 480,
                Height = 480,
                DataContext = new ClearMineViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }.Show();
        }
    }
}
