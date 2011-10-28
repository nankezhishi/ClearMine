namespace ClearMine.Themes
{
    using System;
    using System.Windows;
    using ClearMine.Common.Modularity;

    public class ThemeModule : IModule
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
                Source = new Uri("/ClearMine.Themes;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute),
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("/ClearMine.Themes;component/Themes/luna.normalcolor.xaml", UriKind.RelativeOrAbsolute),
            });
        }
    }
}
