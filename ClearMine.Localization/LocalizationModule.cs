namespace ClearMine.Localization
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;
    using System.Xaml;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Utilities;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizationModule : IModule
    {
        private static int languageResourceIndex;

        public void InitializeModule()
        {
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
        }

        protected Collection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }

        private void OnSwitchLanguage(SwitchLanguageMessage message)
        {
            var path = String.Empty;
            message.HandlingResult = false;

            if (SwitchLanguageMessage.CustomLanguageKey.Equals(message.CultureName, StringComparison.Ordinal))
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
                path = "/ClearMine.Localization;component/{0}/Overall.xaml".InvariantFormat(message.CultureName);
            }

            try
            {
                var languageDictionary = path.MakeResDic();
                if (Resources[languageResourceIndex].VerifyResources(languageDictionary, typeof(string)))
                {
                    Resources[languageResourceIndex] = languageDictionary;
                }
            }
            catch (XamlParseException ex)
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
                Resources.Add("/ClearMine.Localization;component/{0}/Overall.xaml".MakeResDic(cultureName));
            }
            catch (Exception)
            {
                // Load English by default when the resources for current culture not available.
                Resources.Add("/ClearMine.Localization;component/en-US/Overall.xaml".MakeResDic());
            }

            languageResourceIndex = Application.Current.Resources.MergedDictionaries.Count - 1;
        }
    }
}
