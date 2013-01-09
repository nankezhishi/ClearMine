namespace ClearMine.Framework
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// 
    /// </summary>
    public static class UICommandBindings
    {
        #region Close Command
        private static CommandBinding closeBinding = new CommandBinding(ApplicationCommands.Close, OnCloseExecuted);

        private static void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var window = Window.GetWindow(sender as DependencyObject);
            if (window != null)
                window.Close();
        }
        public static CommandBinding CloseBinding { get { return closeBinding; } }
        #endregion
        #region Maximize Command
        private static CommandBinding maximizeBinding = new CommandBinding(UICommands.Maximize, OnMaximizeExecuted);

        private static void OnMaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var window = Window.GetWindow(sender as DependencyObject);
            if (window != null)
            {
                if (window.WindowState == WindowState.Maximized)
                    window.WindowState = WindowState.Normal;
                else if (window.WindowState == WindowState.Normal)
                    window.WindowState = WindowState.Maximized;
            }
        }
        public static CommandBinding MaximizeBinding { get { return maximizeBinding; } }
        #endregion
        #region Minimize Command
        private static CommandBinding minimizeBinding = new CommandBinding(UICommands.Minimize, OnMinimizeExecuted);

        private static void OnMinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var window = Window.GetWindow(sender as DependencyObject);
            if (window != null)
                window.WindowState = WindowState.Minimized;
        }
        public static CommandBinding MinimizeBinding { get { return minimizeBinding; } }
        #endregion
    }
}
