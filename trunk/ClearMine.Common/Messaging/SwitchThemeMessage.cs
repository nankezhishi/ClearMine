namespace ClearMine.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class SwitchThemeMessage : MessageBase
    {
        private const string customThemeKey = "Custom";

        public static string CustomThemeKey
        {
            get { return customThemeKey; }
        }

        public SwitchThemeMessage(string theme)
        {
            ThemeName = theme;
        }

        public SwitchThemeMessage(string theme, object source)
            : this(theme)
        {
            Source = source;
        }

        public string ThemeName { get; set; }
    }
}
