namespace ClearMine.Localization
{
    using System;
    using System.Globalization;
    using System.Linq;
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
        public void InitializeModule()
        {
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
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
                    Filter = LocalizationHelper.FindText("LanguageFileFilter"),
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
                path = String.Format(CultureInfo.InvariantCulture,
                    "/ClearMine.Localization;component/{0}/Overall.xaml", message.CultureName);
            }

            try
            {
                ResourceDictionary languageDictionary = new ResourceDictionary()
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };

                if (VerifyLanguageResourceFile(Application.Current.Resources.MergedDictionaries[0], languageDictionary))
                {
                    Application.Current.Resources.MergedDictionaries[0] = languageDictionary;
                }
            }
            catch (XamlParseException ex)
            {
                var msg = String.Format(CultureInfo.InvariantCulture,
                    LocalizationHelper.FindText("LanguageResourceParseError"), ex.Message);
                message.HandlingResult = true;
                MessageBox.Show(msg, LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);

            try
            {
                var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri(String.Format(CultureInfo.InvariantCulture, "/ClearMine.Localization;component/{0}/Overall.xaml", cultureName), UriKind.RelativeOrAbsolute),
                });
            }
            catch (Exception)
            {
                // Load English by default when the resources for current culture not available.
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("/ClearMine.Localization;component/en-US/Overall.xaml", UriKind.RelativeOrAbsolute),
                });
            }
        }

        private static bool VerifyLanguageResourceFile(ResourceDictionary existing, ResourceDictionary newResource)
        {
            foreach (var resource in newResource.Values)
            {
                if (!(resource is string))
                {
                    MessageBox.Show(LocalizationHelper.FindText("InvalidLanguageResourceType"),
                        LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            var newKeys = newResource.Keys.Cast<string>();

            foreach (var existingKey in existing.Keys)
            {
                if (!newKeys.Contains(existingKey))
                {
                    var message = String.Format(CultureInfo.InvariantCulture,
                        LocalizationHelper.FindText("MissingLanguageResourceKey"), existingKey);
                    MessageBox.Show(message, LocalizationHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
