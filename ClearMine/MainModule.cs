namespace ClearMine
{
    using System.Threading;
    using System.Windows;

    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.VM;

    internal class MainModule : IModule
    {
        /// <summary>
        /// 
        /// </summary>
        public void InitializeModule()
        {
            if (Settings.Default.CurrentLanguage == null)
                Settings.Default.CurrentLanguage = Thread.CurrentThread.CurrentUICulture.Name;

            Application.Current.Startup += OnApplicationStartup;
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= OnApplicationStartup;

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
