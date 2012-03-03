namespace ClearMine.VM
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;

    internal class MainModule : ClearMine.Common.Modularity.IModule
    {
        /// <summary>
        /// 
        /// </summary>
        public void InitializeModule()
        {
            Application.Current.Startup += OnApplicationStartup;
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= OnApplicationStartup;

            InitializeMenu();
            var window = new Window()
            {
                DataContext = new ClearMineViewModel(),
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.SetResourceReference(Window.StyleProperty, "MainWindowStyle");
            window.SetResourceReference(Window.ContentTemplateProperty, typeof(ClearMineViewModel));
            window.Show();
        }

        private static void InitializeMenu()
        {
            Infrastructure.MenuDefinition.Add(new MenuItemData("GameMenuHeader")
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
            Infrastructure.MenuDefinition.Add(new MenuItemData("ToolsMenuHeader")
            {
                SubMenus = new ObservableCollection<object>()
                {
                    new MenuItemData(),
                    new MenuItemData("OptionsMenuItemHeader", GameCommands.Option),
                    new MenuItemData("PluginsMenuItemHeader", GameCommands.Plugins),
                },
            });
            Infrastructure.MenuDefinition.Add(new MenuItemData("HelpMenuItemHeader")
            {
                SubMenus = new ObservableCollection<object>()
                {
                    new MenuItemData("ViewHelpMenuItemHeader", ApplicationCommands.Help),
                    new MenuItemData("SendFeedbakMenuItemHeader", GameCommands.Feedback),
                    new MenuItemData(),
                    new MenuItemData("AboutMenuItemHeader", GameCommands.About)
                },
            });
            MessageManager.SendMessage<UIComponentLoadedMessage>("MainMenu");
        }
    }
}
