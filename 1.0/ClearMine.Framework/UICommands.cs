namespace ClearMine.Framework
{
    using System.Windows.Input;

    /// <summary>
    /// 
    /// </summary>
    public class UICommands
    {
        private static ICommand minimize = new RoutedUICommand("Minimize", "Minimize", typeof(UICommands));

        /// <summary>
        /// 
        /// </summary>
        public static ICommand Minimize
        {
            get { return minimize; }
        }

        private static ICommand maximize = new RoutedUICommand("Maximize", "Maximize", typeof(UICommands));

        /// <summary>
        /// 
        /// </summary>
        public static ICommand Maximize
        {
            get { return maximize; }
        }
    }
}
