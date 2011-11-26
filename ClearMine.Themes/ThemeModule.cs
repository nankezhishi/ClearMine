namespace ClearMine.Themes
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    using ClearMine.Common.Modularity;
    using ClearMine.Common.Utilities;

    public class ThemeModule : IModule
    {
        private ThemeSwitcher themeSwitcher;

        public void InitializeModule()
        {
            themeSwitcher = new ThemeSwitcher("/ClearMine.Themes;component/Themes/{0}.xaml",
                new[] { typeof(Rect), typeof(Brush), typeof(Style), typeof(DataTemplate), typeof(ShaderEffect) }, true);
        }
    }
}
