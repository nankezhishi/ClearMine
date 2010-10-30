namespace ClearMine.Logic
{
    using System;
    using System.Globalization;
    using ClearMine.Common.ComponentModel;

    internal class MineCell : BindableObject
    {
        private CellState state;
        private int minesNearBy;

        public MineCell(int column, int row)
        {
            UpdatePosition(column, row);
            State = CellState.Normal;
        }

        public event EventHandler StateChanged;

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
            set
            {
                if (state != value)
                {
                    SetProperty(ref state, value, "State");
                    var temp = StateChanged;
                    if (temp != null)
                    {
                        temp(this, EventArgs.Empty);
                    }
                }
            }
        }

        public void UpdatePosition(int column, int row)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0}, {1}) {3} : {2}", Column, Row, MinesNearby, HasMine);
        }
    }
}
