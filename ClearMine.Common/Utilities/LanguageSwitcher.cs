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
        /// <param name="stringFormat"></param>
        /// <param name="validTypes"></param>
        /// <param name="supportCustom"></param>
        public LanguageSwitcher(string stringFormat, Type[] validTypes, bool supportCustom)
            : base(stringFormat, validTypes, supportCustom)
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
                if (supportCustom)
                {
                    resourceString = ShowUpOpenResourceDialog("LanguageFileFilter");
                    if (resourceString == null)
                    {
                        message.HandlingResult = true;
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                resourceString = resourceStringFormat.InvariantFormat(message.CultureName);
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

            Resources.Add(resourceStringFormat.MakeResDic(cultureName));
        }
    }
}
