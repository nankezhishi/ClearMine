namespace ClearMine
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ClearMine.Common.Properties;
    using ClearMine.Framework.Dialogs;
    using ClearMine.VM;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool IsSettingsDirty { get; set; }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += OnCurrentDispatcherUnhandledException;
            Exit += new ExitEventHandler(OnApplicationExit);
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
            Settings.Default.SettingsSaving += new System.Configuration.SettingsSavingEventHandler(OnSavingSettings);
            var mainWindow = new Window()
            {
                DataContext = new ClearMineViewModel(),
                Width = 640,
                Height = 480,
                Background = Brushes.Silver,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            TextOptions.SetTextFormattingMode(mainWindow, TextFormattingMode.Display);
            mainWindow.SetResourceReference(Window.TitleProperty, "ApplicationTitle");
            mainWindow.SetBinding(Window.ContentProperty, ".");
            mainWindow.Show();
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (IsSettingsDirty)
            {
                Settings.Default.Save();
            }
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            IsSettingsDirty = true;
        }

        private void OnSavingSettings(object sender, CancelEventArgs e)
        {
            IsSettingsDirty = e.Cancel;
        }

        private void OnCurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = ExceptionBox.Show(e.Exception).Value;
        }
    }
}
