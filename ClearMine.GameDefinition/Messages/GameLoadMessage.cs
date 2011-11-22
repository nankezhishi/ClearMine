namespace ClearMine.GameDefinition.Messages
{
    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class GameLoadMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameLoadMessage(IClearMine game)
        {
            NewGame = game;
        }

        /// <summary>
        /// 
        /// </summary>
        public IClearMine NewGame { get; set; }
    }
}
