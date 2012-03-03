namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.Generic;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;

    /// <summary>
    /// 
    /// </summary>
    public class ThemeSwitcher : ResourceSwitcher
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> DefaultThemes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceFormat"></param>
        /// <param name="validTypes"></param>
        /// <param name="supportCustom"></param>
        public ThemeSwitcher(string resourceFormat, Type[] validTypes, bool supportCustom)
            : base(resourceFormat, validTypes, supportCustom)
        {
            MessageManager.SubscribeMessage<SwitchThemeMessage>(OnSwitchTheme);
        }

        private void OnSwitchTheme(SwitchThemeMessage message)
        {
            if (message.HandlingResult == null)
            {
                message.HandlingResult = false;
            }
            var resourceString = message.ThemeName;
            if (SwitchThemeMessage.CustomThemeKey.Equals(message.ThemeName, StringComparison.Ordinal))
            {
                if (SupportCustom)
                {
                    resourceString = ShowUpOpenResourceDialog("ThemeFileFilter");
                    if (resourceString == null)
                    {
                        message.HandlingResult = true;
                        return;
                    }
                    else
                    {
                        Settings.Default.CustomThemeFile = resourceString;
                    }
                }
                else
                {
                    resourceString = ResourceFormat.InvariantFormat(Settings.Default.CurrentTheme);
                }
            }
            else
            {
                resourceString = ResourceFormat.InvariantFormat(message.ThemeName);
            }

            if (SwitchResource(resourceString))
            {
                message.HandlingResult = true;
            }
            else
            {
                Settings.Default.CurrentTheme = message.ThemeName;
            }
        }

        protected override void OnApplicationStartup()
        {
            if (DefaultThemes != null)
            {
                foreach (var resource in DefaultThemes)
                {
                    Resources.Add(resource.MakeResource());
                }
            }
        }
    }
}
