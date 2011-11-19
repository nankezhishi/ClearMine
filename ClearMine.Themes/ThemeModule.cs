namespace ClearMine.Themes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Utilities;
    using Microsoft.Win32;

    public class ThemeModule : IModule
    {
        private static int themeResourceIndex;

        protected Collection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }

        public void InitializeModule()
        {
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
            MessageManager.SubscribeMessage<SwitchThemeMessage>(OnSwitchTheme);
        }

        private void OnSwitchTheme(SwitchThemeMessage message)
        {
            message.HandlingResult = false;
            var themeString = message.ThemeName;
            if (SwitchThemeMessage.CustomThemeKey.Equals(message.ThemeName))
            {
                var openFileDialog = new OpenFileDialog()
                {
                    DefaultExt = ".xaml",
                    CheckFileExists = true,
                    Multiselect = false,
                    Filter = ResourceHelper.FindText("ThemeFileFilter"),
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    themeString = openFileDialog.FileName;
                }
                else
                {
                    message.HandlingResult = true;
                    return;
                }
            }
            else
            {
                themeString = "/ClearMine.Themes;component/Themes/{0}.xaml".InvariantFormat(message.ThemeName);
            }

            try
            {
                var themeDictionary = themeString.MakeResDic();
                if (Resources[themeResourceIndex].VerifyResources(themeDictionary,
                    typeof(Rect), typeof(Brush), typeof(Style), typeof(DataTemplate), typeof(ShaderEffect)))
                {
                    Resources[themeResourceIndex] = themeDictionary;
                }
                else
                {
                    message.HandlingResult = true;
                }
            }
            catch (Exception ex)
            {
                var msg = ResourceHelper.FindText("ResourceParseError", ex.Message);
                message.HandlingResult = true;
                MessageBox.Show(msg, ResourceHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);
            Resources.Add("/ClearMine.Themes;component/Themes/Generic.xaml".MakeResDic());
            Resources.Add("/ClearMine.Themes;component/Themes/luna.normalcolor.xaml".MakeResDic());
            themeResourceIndex = Resources.Count - 1;
        }
    }
}
