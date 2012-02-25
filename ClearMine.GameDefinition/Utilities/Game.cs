namespace ClearMine.GameDefinition.Utilities
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;

    /// <summary>
    /// 
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// 
        /// </summary>
        public static ObservableCollection<MenuItemData> MenuDefinition { get; private set; }

        static Game()
        {
            MenuDefinition = new ObservableCollection<MenuItemData>();
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
