namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;

    using ClearMine.Common.Messaging;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    public class LanguageSwitcher
    {
        protected int languageResourceIndex;
        protected string resourceStringFormat;
        protected Type[] validResourceTypes;
        protected bool supportCustomLanguage;

        protected Collection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }

        public LanguageSwitcher(string stringFormat, Type[] validTypes, bool supportCustom)
        {
            validResourceTypes = validTypes;
            resourceStringFormat = stringFormat;
            supportCustomLanguage = supportCustom;
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
        }

        private void OnSwitchLanguage(SwitchLanguageMessage message)
        {
            var path = String.Empty;
            message.HandlingResult = false;

            if (SwitchLanguageMessage.CustomLanguageKey.Equals(message.CultureName, StringComparison.Ordinal))
            {
                if (supportCustomLanguage)
                {
                    var openFileDialog = new OpenFileDialog()
                    {
                        DefaultExt = ".xaml",
                        CheckFileExists = true,
                        Multiselect = false,
                        Filter = ResourceHelper.FindText("LanguageFileFilter"),
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        path = openFileDialog.FileName;
                    }
                    else
                    {
                        message.HandlingResult = true;
                        return;
                    }
                }
                else
                {
                    path = resourceStringFormat.InvariantFormat(Thread.CurrentThread.CurrentUICulture.Name);
                }
            }
            else
            {
                path = resourceStringFormat.InvariantFormat(message.CultureName);
            }

            try
            {
                var languageDictionary = path.MakeResDic();
                if (Resources[languageResourceIndex].VerifyResources(languageDictionary, validResourceTypes))
                {
                    Resources[languageResourceIndex] = languageDictionary;
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

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);

            try
            {
                var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
                Resources.Add(resourceStringFormat.MakeResDic(cultureName));
            }
            catch (Exception)
            {
                // Load English by default when the resources for current culture not available.
                Resources.Add(resourceStringFormat.MakeResDic());
            }

            languageResourceIndex = Application.Current.Resources.MergedDictionaries.Count - 1;
        }
    }
}
