namespace ClearMine
{
    using System.Windows;
    using System.Windows.Threading;

    using ClearMine.UI.Dialogs;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += OnCurrentDispatcherUnhandledException;

            var mainWindow = new ClearMineWindow();
            mainWindow.Show();
        }

        private void OnCurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Exception");
            e.Handled = true;
        }
    }
}
