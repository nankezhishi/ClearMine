namespace ClearMine.Common.ComponentModel.UI
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using ClearMine.Common.Messaging;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class MenuItemData : BindableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSeperator"></param>
        public MenuItemData(bool isSeperator = true)
        {
            IsSeperator = isSeperator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerkey"></param>
        /// <param name="command"></param>
        public MenuItemData(string headerkey, ICommand command = null)
        {
            Command = command;
            HeaderKey = headerkey;
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSeperator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsChecked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeaderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Header
        {
            get
            {
                if (HeaderKey == null)
                    return null;

                return ResourceHelper.FindText(HeaderKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual object CommandParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<object> SubMenus { get; set; }

        private void OnSwitchLanguage(SwitchLanguageMessage message)
        {
            TriggerPropertyChanged("Header");
        }
    }
}
