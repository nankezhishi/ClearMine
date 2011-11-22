namespace ClearMine.GameDefinition.Messages
{
    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class GameStateMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameStateMessage(IClearMine game)
        {
            Source = game;
        }
    }
}
