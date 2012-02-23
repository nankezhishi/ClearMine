namespace ClearMine.Common.ComponentModel.UI
{
    using System.Collections.Generic;
    using System.Windows.Input;

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
        }

        public bool IsSeperator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeaderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> SubMenus { get; set; }
    }
}
