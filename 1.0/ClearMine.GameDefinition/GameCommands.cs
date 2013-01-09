namespace ClearMine.GameDefinition
{
    using System.Windows.Input;

    public static class GameCommands
    {
        private static ICommand showStatistics = new RoutedUICommand("Statistics", "Statistics",
            typeof(GameCommands), new InputGestureCollection() { new KeyGesture(Key.F4) });

        public static ICommand ShowStatistics
        {
            get { return showStatistics; }
        }

        private static ICommand about = new RoutedUICommand("About", "About", typeof(GameCommands));

        public static ICommand About
        {
            get { return about; }
        }

        private static ICommand feedback = new RoutedUICommand("Feedback", "Feedback", typeof(GameCommands),
            new InputGestureCollection() { new KeyGesture(Key.F, ModifierKeys.Control) });

        public static ICommand Feedback
        {
            get { return feedback; }
        }

        private static ICommand showLog = new RoutedUICommand("Show Log", "ShowLog",
            typeof(GameCommands), new InputGestureCollection() { new KeyGesture(Key.L, ModifierKeys.Control) });

        public static ICommand ShowLog
        {
            get { return showLog; }
        }

        private static ICommand switchLanguage = new RoutedUICommand("Change Language", "ChangeLanguage", typeof(GameCommands));

        public static ICommand SwitchLanguage
        {
            get { return switchLanguage; }
        }

        private static ICommand switchTheme = new RoutedUICommand("Change Theme", "ChangeTheme", typeof(GameCommands));

        public static ICommand SwitchTheme
        {
            get { return switchTheme; }
        }

        private static ICommand plugins = new RoutedUICommand("Plugins", "Plugins", typeof(GameCommands),
            new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control) });

        public static ICommand Plugins
        {
            get { return plugins; }
        }

        private static ICommand option = new RoutedUICommand("Option", "Option", typeof(GameCommands),
            new InputGestureCollection() { new KeyGesture(Key.P, ModifierKeys.Control) });

        public static ICommand Option
        {
            get { return option; }
        }

        private static ICommand browseHistory = new RoutedUICommand("BrowseHistory", "BrowseHistory", typeof(GameCommands),
            new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control) });

        public static ICommand BrowseHistory
        {
            get { return browseHistory; }
        }

        private static ICommand reset = new RoutedUICommand("Reset", "Reset", typeof(GameCommands));

        public static ICommand Reset
        {
            get { return reset; }
        }
    }
}
