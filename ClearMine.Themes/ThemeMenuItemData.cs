namespace ClearMine.Themes
{
    using System;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;

    public class ThemeMenuItemData : MenuItemData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerkey"></param>
        public ThemeMenuItemData(string headerkey, ICommand command)
            : base(headerkey, command)
        {
            MessageManager.SubscribeMessage<SwitchThemeMessage>(OnSwitchTheme, ListeningPriority.Last);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsChecked
        {
            get { return Settings.Default.CurrentTheme == CommandParameter as string; }
            set { throw new InvalidOperationException(); }
        }

        private void OnSwitchTheme(SwitchThemeMessage message)
        {
            TriggerPropertyChanged("IsChecked");
        }
    }
}
