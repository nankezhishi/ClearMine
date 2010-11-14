namespace ClearMine.Framework.Commands
{
    using System.Windows.Input;

    public class OptionsCommands
    {
        private static ICommand browseHistory = new RoutedUICommand("BrowseHistory", "BrowseHistory",
            typeof(OptionsCommands), new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control) });

        public static ICommand BrowseHistory
        {
            get { return browseHistory; }
        }
    }
}
