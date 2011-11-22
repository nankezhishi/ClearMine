namespace ClearMine.GameDefinition.Messages
{
    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class CellStatusMessage : MessageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        public CellStatusMessage(MineCell cell, CellState oldState)
        {
            Cell = cell;
            OldState = oldState;
        }

        /// <summary>
        /// 
        /// </summary>
        public MineCell Cell { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CellState OldState { get; set; }
    }
}
