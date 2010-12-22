namespace ClearMine
{
    using System.Windows;
    using System.Windows.Media;

    using ClearMine.Common.Properties;
    using ClearMine.Framework.Dialogs;
    using ClearMine.VM;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool IsSettingsDirty { get; private set; }

        public App()
        {
            DispatcherUnhandledException += (sender, e) => e.Handled = ExceptionBox.Show(e.Exception).Value;
            Settings.Default.PropertyChanged += (sender, e) => IsSettingsDirty = true;
            Settings.Default.SettingsSaving += (sender, e) => IsSettingsDirty = e.Cancel;
        }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new Window()
            {
                Width = 480,
                Height = 480,
                Background = Brushes.Silver,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TextOptions.SetTextFormattingMode(mainWindow, TextFormattingMode.Display);
            mainWindow.SetResourceReference(Window.TitleProperty, "ApplicationTitle");
            mainWindow.SetBinding(Window.ContentProperty, ".");
            mainWindow.Show();
            mainWindow.DataContext = new ClearMineViewModel();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (IsSettingsDirty)
            {
                Settings.Default.Save();
            }
        }
    }
}
