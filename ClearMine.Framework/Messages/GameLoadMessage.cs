namespace ClearMine.Framework.Messages
{
    using ClearMine.Common.Messaging;
    using ClearMine.GameDefinition;

    /// <summary>
    /// 
    /// </summary>
    public class GameLoadMessage : MessageBase
    {
        public GameLoadMessage(IClearMine game)
        {
            NewGame = game;
        }

        public IClearMine NewGame { get; set; }
    }
}
