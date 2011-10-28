namespace ClearMine.Localization
{
    using System;
    using System.Windows;
    using ClearMine.Common.Modularity;

    public class LocalizationModule : IModule
    {
        public void InitializeModule()
        {
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
        }

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("/ClearMine.Localization;component/en-US/Overall.xaml", UriKind.RelativeOrAbsolute),
            });
        }
    }
}
