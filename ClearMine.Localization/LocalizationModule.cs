namespace ClearMine.Localization
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition.Commands;
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
    }
}
