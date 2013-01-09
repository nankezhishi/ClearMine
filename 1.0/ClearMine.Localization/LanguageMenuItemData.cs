namespace ClearMine.Localization
{
    using System;
    using System.Windows.Input;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;

    /// <summary>
    /// 
    /// </summary>
    public class LanguageMenuItemData : MenuItemData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerkey"></param>
        public LanguageMenuItemData(string headerkey, ICommand command)
            : base(headerkey, command)
        {
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage, ListeningPriority.Last);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsChecked
        {
            get { return Settings.Default.CurrentLanguage == CommandParameter as string; }
            set { throw new InvalidOperationException(); }
        }

        private void OnSwitchLanguage(SwitchLanguageMessage message)
        {
            TriggerPropertyChanged("IsChecked");
        }
    }
}
