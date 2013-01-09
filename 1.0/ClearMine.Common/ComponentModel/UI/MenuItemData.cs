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
        /// <param name="isSeparator"></param>
        public MenuItemData(bool isSeparator = true)
        {
            IsSeparator = isSeparator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerKey"></param>
        /// <param name="command"></param>
        public MenuItemData(string headerKey, ICommand command = null)
        {
            Command = command;
            HeaderKey = headerKey;
            MessageManager.SubscribeMessage<SwitchLanguageMessage>(OnSwitchLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSeparator { get; set; }

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
