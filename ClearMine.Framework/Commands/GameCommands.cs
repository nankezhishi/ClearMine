namespace ClearMine.Framework.Commands
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

        private static ICommand feedback = new RoutedUICommand("Feedback", "Feedback", typeof(GameCommands));

        public static ICommand Feedback
        {
            get { return feedback; }
        }

        private static ICommand showLog = new RoutedUICommand("ShowLog", "ShowLog",
            typeof(GameCommands), new InputGestureCollection() { new KeyGesture(Key.L, ModifierKeys.Control) });

        public static ICommand ShowLog
        {
            get { return showLog; }
        }
    }
}
