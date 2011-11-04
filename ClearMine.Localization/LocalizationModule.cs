namespace ClearMine.Localization
{
    using System;
    using System.Threading;
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

            try
            {
                var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(String.Format("/ClearMine.Localization;component/{0}/Overall.xaml", cultureName), UriKind.RelativeOrAbsolute),
                });
            }
            catch (Exception)
            {
                // Load English by default when the resources for current culture not available.
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("/ClearMine.Localization;component/en-US/Overall.xaml", UriKind.RelativeOrAbsolute),
                });
            }
        }
    }
}
