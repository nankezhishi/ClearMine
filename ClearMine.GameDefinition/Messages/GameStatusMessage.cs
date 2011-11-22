namespace ClearMine.GameDefinition.Messages
{
    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class GameStatusMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GameStatusMessage(IClearMine game)
        {
            Source = game;
        }
    }
}
