namespace ClearMine
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Threading;

    using ClearMine.Properties;
    using ClearMine.UI.Dialogs;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        public bool IsSettingsDirty { get; set; }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += OnCurrentDispatcherUnhandledException;
            Exit += new ExitEventHandler(OnApplicationExit);
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
            Settings.Default.SettingsSaving += new System.Configuration.SettingsSavingEventHandler(OnSavingSettings);

            var mainWindow = new ClearMineWindow();
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
            MessageBox.Show(e.Exception.Message, "Exception");
            if (MainWindow != null)
            {
                e.Handled = true;
            }
        }
    }
}
