namespace ClearMine
{
    using System.Windows;

    internal class MainModule : ClearMine.Common.Modularity.IModule
    {
        /// <summary>
        /// 
        /// </summary>
        public void InitializeModule()
        {
            Application.Current.Startup += OnApplicationStartup;
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= OnApplicationStartup;

            new Window()
            {
                DataContext = new ClearMine.VM.ClearMineViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }.Show();
        }
    }
}
