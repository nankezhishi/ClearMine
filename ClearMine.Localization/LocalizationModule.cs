namespace ClearMine.Localization
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizationModule : IModule
    {
        private LanguageSwitcher switcher;

        public void InitializeModule()
        {
            switcher = new LanguageSwitcher("/ClearMine.Localization;component/{0}/Overall.xaml", new[] { typeof(string), typeof(ImageSource) }, true);
            switcher.Initialized += new EventHandler<GenericEventArgs<Collection<ResourceDictionary>>>(OnLanguageSwitcherInitailized);
            Game.MenuDefinition[1].SubMenus.Insert(0, 
                new MenuItemData("LanguageMenuHeader")
                {
                    SubMenus = new ObservableCollection<object>()
                    {
                        new LanguageMenuItemData("EnglishMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = "en-US" },
                        new LanguageMenuItemData("ChineseMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = "zh-CN" },
                        new MenuItemData(),
                        new LanguageMenuItemData("CustomLanguageMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = SwitchLanguageMessage.CustomLanguageKey },
                    }
                });
        }

        private void OnLanguageSwitcherInitailized(object sender, GenericEventArgs<Collection<ResourceDictionary>> e)
        {
            // Handling the custom language file for the main application.
            // Not all the switchers need this step.
            if (SwitchLanguageMessage.CustomLanguageKey.Equals(Settings.Default.CurrentLanguage, StringComparison.Ordinal))
            {
                if (File.Exists(Settings.Default.CustomLanguageFile))
                {
                    e.Data.Add(Settings.Default.CustomLanguageFile.MakeResource());
                }
                // 自定义语言文件不存在，则使用当前语言
                else
                {
                    Settings.Default.CurrentLanguage = Thread.CurrentThread.CurrentUICulture.Name;
                    e.Data.Add(switcher.ResourceFormat.MakeResource(Settings.Default.CurrentLanguage));
                }
            }
        }
    }
}
