namespace ClearMine.Logic
{
    using ClearMine.Framework;

    internal class MineCell : BindableObject
    {
        private CellState state;

        public MineCell(int column, int row)
        {
            Row = row;
            Column = column;
            State = CellState.Normal;
        }

        public bool HasMine { get; set; }

        public int MinesNearby { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public CellState State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }
    }
}
