namespace ClearMine.Themes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class ThemeModule : IModule
    {
        private static readonly string themeResourcePattern = "/ClearMine.Themes;component/Themes/{0}.xaml";

        private ThemeSwitcher themeSwitcher;

        public void InitializeModule()
        {
            themeSwitcher = new ThemeSwitcher(themeResourcePattern,
                new[] { typeof(Rect), typeof(Brush), typeof(Style), typeof(DataTemplate), typeof(ShaderEffect) }, true);

            var defaultThemes = new List<string>() { "/ClearMine.Themes;component/Themes/Generic.xaml" };
            if (SwitchThemeMessage.CustomThemeKey.Equals(Settings.Default.CurrentTheme, StringComparison.Ordinal))
            {
                defaultThemes.Add(Settings.Default.CustomThemeFile);
            }
            else
            {
                defaultThemes.Add(String.Format(themeResourcePattern, Settings.Default.CurrentTheme));
            }
            themeSwitcher.DefaultThemes = defaultThemes;
            MessageManager.SubscribeMessage<UIComponentLoadedMessage>(OnUIComponentLoaded);
        }

        private void OnUIComponentLoaded(UIComponentLoadedMessage msg)
        {
            if (msg.ComponentName.Equals("MainMenu", StringComparison.Ordinal))
            {
                Infrastructure.MenuDefinition[1].SubMenus.Insert(0, new MenuItemData("ThemeMenuHeader")
                {
                    SubMenus = new ObservableCollection<object>()
                    {
                        new ThemeMenuItemData("ClassicThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = "luna.normalcolor" },
                        new ThemeMenuItemData("LinuxThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = "linux.ubuntu" },
                        new MenuItemData(),
                        new ThemeMenuItemData("CustomThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = SwitchThemeMessage.CustomThemeKey },
                    },
                });
            }
        }
    }
}
