namespace ClearMine
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Framework.Dialogs;
    using ClearMine.Localization;
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
            ModuleManager.LoadModules();
            Settings.Default.PropertyChanged += (sender, e) => IsSettingsDirty = true;
            Settings.Default.SettingsSaving += (sender, e) => IsSettingsDirty = e.Cancel;
            EventManager.RegisterClassHandler(typeof(Window), FocusManager.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
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

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            Trace.TraceInformation(LocalizationHelper.FindText("FocusChangedMessage", e.OriginalSource));
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
