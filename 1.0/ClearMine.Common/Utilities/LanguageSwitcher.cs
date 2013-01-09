namespace ClearMine.Common.Utilities
{
    using System;
    using System.Threading;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;

    /// <summary>
    /// 
    /// </summary>
    public class LanguageSwitcher : ResourceSwitcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceFormat"></param>
        /// <param name="validTypes"></param>
        /// <param name="supportCustom"></param>
        public LanguageSwitcher(string resourceFormat, Type[] validTypes, bool supportCustom)
            : base(resourceFormat, validTypes, supportCustom)
        {
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
        }

        private void OnSwitchLanguage(SwitchLanguageMessage message)
        {
            if (message.HandlingResult == null)
            {
                message.HandlingResult = false;
            }
            var resourceString = String.Empty;
            if (SwitchLanguageMessage.CustomLanguageKey.Equals(message.CultureName, StringComparison.Ordinal))
            {
                if (SupportCustom)
                {
                    resourceString = ShowUpOpenResourceDialog("LanguageFileFilter");
                    if (resourceString == null)
                    {
                        message.HandlingResult = true;
                        return;
                    }
                    else
                    {
                        Settings.Default.CustomLanguageFile = resourceString;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                resourceString = ResourceFormat.InvariantFormat(message.CultureName);
            }

            if (SwitchResource(resourceString))
            {
                message.HandlingResult = true;
            }
            else
            {
                Settings.Default.CurrentLanguage = message.CultureName;
            }
        }

        protected override void OnApplicationStartup()
        {
            var cultureName = Settings.Default.CurrentLanguage;
            if (String.IsNullOrWhiteSpace(cultureName))
            {
                cultureName = Thread.CurrentThread.CurrentUICulture.Name;
                Settings.Default.CurrentLanguage = cultureName;
            }
            // A general language switch not know about how to initialize custom lanugage files.
            else if (SwitchLanguageMessage.CustomLanguageKey.Equals(cultureName, StringComparison.Ordinal))
            {
                // If the switch knows how, then let it go.
                if (SupportCustom)
                {
                    return;
                }
                // use default.
                else
                {
                    cultureName = Thread.CurrentThread.CurrentUICulture.Name;
                }
            }

            Resources.Add(ResourceFormat.MakeResource(cultureName));
        }
    }
}
