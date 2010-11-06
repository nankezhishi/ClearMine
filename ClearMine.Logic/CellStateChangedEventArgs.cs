namespace ClearMine.Logic
{
    using System;

    public class CellStateChangedEventArgs : EventArgs
    {
        public MineCell Cell { get; set; }

        public CellState OldState { get; set; }

        public CellStateChangedEventArgs(MineCell cell, CellState oldState)
        {
            Cell = cell;
            OldState = oldState;
        }
    }
}
