namespace ClearMine.GameDefinition.Messages
{
    using System.Collections.Generic;
    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class UserOperationMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        public UserOperationMessage(IClearMine game, GameOperation operation, IEnumerable<MineCell> effectedCells)
        {
            Source = game;
            UserOperation = operation;
            EffectedCells = effectedCells;
        }

        /// <summary>
        /// 
        /// </summary>
        public GameOperation UserOperation { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<MineCell> EffectedCells { get; protected set; }
    }
}
