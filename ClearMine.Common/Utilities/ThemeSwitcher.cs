namespace ClearMine.Common.Utilities
{
    using System;
    using System.Windows;

    using ClearMine.Common.Messaging;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    public class ThemeSwitcher : ResourceSwitcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringFormat"></param>
        /// <param name="validTypes"></param>
        /// <param name="supportCustom"></param>
        public ThemeSwitcher(string stringFormat, Type[] validTypes, bool supportCustom)
            : base(stringFormat, validTypes, supportCustom)
        {
            MessageManager.SubscribeMessage<SwitchThemeMessage>(OnSwitchTheme);
        }

        private void OnSwitchTheme(SwitchThemeMessage message)
        {
            message.HandlingResult = false;
            var themeString = message.ThemeName;
            if (SwitchThemeMessage.CustomThemeKey.Equals(message.ThemeName))
            {
                if (supportCustom)
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
                    themeString = resourceStringFormat.InvariantFormat("luna.normalcolor");
                }
            }
            else
            {
                themeString = resourceStringFormat.InvariantFormat(message.ThemeName);
            }

            try
            {
                var themeDictionary = themeString.MakeResDic();
                if (Resources[resourceIndex].VerifyResources(themeDictionary, validResourceTypes))
                {
                    Resources[resourceIndex] = themeDictionary;
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

        protected override void OnApplicationStartup()
        {
            Resources.Add("/ClearMine.Themes;component/Themes/Generic.xaml".MakeResDic());
            Resources.Add("/ClearMine.Themes;component/Themes/luna.normalcolor.xaml".MakeResDic());
        }
    }
}
