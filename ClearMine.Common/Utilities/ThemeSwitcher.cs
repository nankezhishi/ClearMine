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
            if (message.HandlingResult == null)
            {
                message.HandlingResult = false;
            }
            var resourceString = message.ThemeName;
            if (SwitchThemeMessage.CustomThemeKey.Equals(message.ThemeName))
            {
                if (supportCustom)
                {
                    resourceString = ShowUpOpenResourceDialog("ThemeFileFilter");
                    if (resourceString == null)
                    {
                        message.HandlingResult = true;
                        return;
                    }
                }
                else
                {
                    resourceString = resourceStringFormat.InvariantFormat(Settings.Default.CurrentTheme);
                }
            }
            else
            {
                resourceString = resourceStringFormat.InvariantFormat(message.ThemeName);
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
            foreach (var resource in DefaultThemes)
            {
                Resources.Add(resource.MakeResDic());
            }
        }
    }
}
