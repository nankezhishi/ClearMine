namespace ClearMine.GameDefinition.Utilities
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.GameDefinition.Commands;

    /// <summary>
    /// 
    /// </summary>
    public class Game
    {
        /// <summary>
        /// 
        /// </summary>
        public static ObservableCollection<object> MenuDefinition { get; private set; }

        static Game()
        {
            MenuDefinition = new ObservableCollection<object>();
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            InitializeMenu();
        }

        private static void InitializeMenu()
        {
            MenuDefinition.Add(new MenuItemData("GameMenuHeader")
            {
                SubMenus = new ObservableCollection<object>()
                {
                    new MenuItemData("NewGameMenuHeader", ApplicationCommands.New),
                    new MenuItemData("RestartGameMenuHeader", NavigationCommands.Refresh),
                    new MenuItemData(),
                    new MenuItemData("OpenGameMenuHeader", ApplicationCommands.Open),
                    new MenuItemData("SaveAsGameMenuHeader", ApplicationCommands.SaveAs),
                    new MenuItemData(),
                    new MenuItemData("HeroListMenuHeader", GameCommands.ShowStatistics),
                    new MenuItemData(),
                    new MenuItemData("ExitMenuHeader", ApplicationCommands.Close),
                },
            });
            MenuDefinition.Add(new MenuItemData("ToolsMenuHeader")
            {
                SubMenus = new ObservableCollection<object>()
                {
                    new MenuItemData("ThemeMenuHeader")
                    {
                        SubMenus = new ObservableCollection<object>()
                        {
                            new MenuItemData("ClassicThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = "luna.normalcolor" },
                            new MenuItemData("LinuxThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = "linux.ubuntu" },
                            new MenuItemData(),
                            new MenuItemData("CustomThemeMenuHeader", GameCommands.SwitchTheme) { CommandParameter = SwitchThemeMessage.CustomThemeKey },
                        },
                    },
                    new MenuItemData("LanguageMenuHeader")
                    {
                        SubMenus = new ObservableCollection<object>()
                        {
                            new MenuItemData("EnglishMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = "en-US" },
                            new MenuItemData("ChineseMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = "zh-CN" },
                            new MenuItemData(),
                            new MenuItemData("CustomLanguageMenuItemHeader", GameCommands.SwitchLanguage) { CommandParameter = SwitchLanguageMessage.CustomLanguageKey },
                        }
                    },
                    new MenuItemData(),
                    new MenuItemData("OptionsMenuItemHeader", GameCommands.Option),
                    new MenuItemData("PluginsMenuItemHeader", GameCommands.Plugins),
                },
            });
            MenuDefinition.Add(new MenuItemData("HelpMenuItemHeader")
            {
                SubMenus = new ObservableCollection<object>()
                {
                    new MenuItemData("ViewHelpMenuItemHeader", ApplicationCommands.Help),
                    new MenuItemData("SendFeedbakMenuItemHeader", GameCommands.Feedback),
                    new MenuItemData(),
                    new MenuItemData("AboutMenuItemHeader", GameCommands.About)
                },
            });
        }
    }
}
