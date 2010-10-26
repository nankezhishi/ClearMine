namespace ClearMine.Logic
{
    using System;
    using System.Globalization;
    using ClearMine.Framework.ComponentModel;

    internal class MineCell : BindableObject
    {
        private CellState state;
        private int minesNearBy;

        public MineCell(int column, int row)
        {
            Row = row;
            Column = column;
            State = CellState.Normal;
        }

        public bool HasMine { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public int MinesNearby
        {
            get { return minesNearBy; }
            set { SetProperty(ref minesNearBy, value); }
        }

        public CellState State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0}, {1}) : {2}", Column, Row, MinesNearby);
        }
    }
}
