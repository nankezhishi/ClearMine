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
            UpdatePosition(column, row);
            State = CellState.Normal;
        }

        public bool HasMine { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public int MinesNearby
        {
            get { return minesNearBy; }
            set
            {
                // Must provide propertyName here, 
                // Otherwise it cause series performance issue. 
                // Because during the initalization of the Game.
                // This property will be set many many times.
                SetProperty(ref minesNearBy, value, "MinesNearby");
            }
        }

        public CellState State
        {
            get { return state; }
            set { SetProperty(ref state, value, "State"); }
        }

        public void UpdatePosition(int column, int row)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0}, {1}) : {2}", Column, Row, MinesNearby);
        }
    }
}
