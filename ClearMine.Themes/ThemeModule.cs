namespace ClearMine.Themes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition.Commands;
    using ClearMine.GameDefinition.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class ThemeModule : IModule
    {
        private ThemeSwitcher themeSwitcher;

        public void InitializeModule()
        {
            themeSwitcher = new ThemeSwitcher("/ClearMine.Themes;component/Themes/{0}.xaml",
                new[] { typeof(Rect), typeof(Brush), typeof(Style), typeof(DataTemplate), typeof(ShaderEffect) }, true);

            themeSwitcher.DefaultThemes = new[]
            {
                "/ClearMine.Themes;component/Themes/Generic.xaml",
                String.Format("/ClearMine.Themes;component/Themes/{0}.xaml", Settings.Default.CurrentTheme)
            };

            Game.MenuDefinition[1].SubMenus.Insert(0,
                new MenuItemData("ThemeMenuHeader")
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
