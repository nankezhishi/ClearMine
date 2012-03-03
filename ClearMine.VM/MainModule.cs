namespace ClearMine.VM
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

            var window = new Window()
            {
                DataContext = new ClearMineViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.SetResourceReference(Window.StyleProperty, "MainWindowStyle");
            window.SetResourceReference(Window.ContentTemplateProperty, typeof(ClearMineViewModel));
            window.Show();
        }
    }
}
