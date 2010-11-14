namespace ClearMine.Framework.Commands
{
    using System.Windows.Input;

    public class StatisticsCommands
    {
        private static ICommand reset = new RoutedUICommand("Reset", "Reset", typeof(StatisticsCommands));

        public static ICommand Reset
        {
            get { return reset; }
        }
    }
}
