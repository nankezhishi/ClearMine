namespace ClearMine.Common.Utilities
{
    using System;

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
                    resourceString = null;
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
        }

        protected override void OnApplicationStartup()
        {
            var cultureName = Settings.Default.CurrentLanguage;
            Resources.Add(resourceStringFormat.MakeResDic(cultureName));
        }
    }
}
