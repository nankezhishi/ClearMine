namespace ClearMine.Framework
{
    using System.ComponentModel;
    using System.Windows;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Common.Properties;
    using ClearMine.Framework.Messages;

    public class FrameworkModule : IModule
    {
        private bool propertyChanged = false;

        public void InitializeModule()
        {
            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                e.Handled = (bool)MessageManager.SendMessage<ExceptionMessage>(e.Exception);
            };

            MessageManager.SubscribeMessage<ExceptionMessage>(new ExceptionMessageProcessor().HandleMessage);
            MessageManager.SubscribeMessage<HelpRequestedMessage>(new HelpRequestedMessageProcessor().HandleMessage);

            // Auto save settings if any setting changes when application exit.
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsPropertyChanged);
            Application.Current.Exit += new ExitEventHandler(OnApplicationExit);
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (propertyChanged)
            {
                Settings.Default.Save();
            }
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Settings.Default.PropertyChanged -= new PropertyChangedEventHandler(OnSettingsPropertyChanged);

            propertyChanged = true;
        }
    }
}
