namespace ClearMine.Framework.Messages
{
    using ClearMine.Common.Messaging;
    using ClearMine.GameDefinition;

    /// <summary>
    /// 
    /// </summary>
    public class GameLoadMessage : MessageBase
    {
        public IClearMine NewGame { get; set; }
    }
}
