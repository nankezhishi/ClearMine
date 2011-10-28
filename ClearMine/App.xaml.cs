namespace ClearMine
{
    using System.Windows;
    using System.Windows.Media;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
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
            DispatcherUnhandledException += (sender, e) =>
            {
                var message = new ExceptionMessage(e.Exception);
                MessageManager.GetMessageAggregator<ExceptionMessage>().SendMessage(message);
                e.Handled = (bool)message.HandlingResult;
            };
            ModuleManager.LoadModules();
            Settings.Default.PropertyChanged += (sender, e) => IsSettingsDirty = true;
            Settings.Default.SettingsSaving += (sender, e) => IsSettingsDirty = e.Cancel;
        }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new Window()
            {
                Width = 480,
                Height = 480,
                UseLayoutRounding = true,
                Background = Brushes.Silver,
                DataContext = new ClearMineViewModel(),
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
    }
}
