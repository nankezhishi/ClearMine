namespace ClearMine.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponentLoadedMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public UIComponentLoadedMessage(string name)
        {
            ComponentName = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ComponentName { get; set; }
    }
}
