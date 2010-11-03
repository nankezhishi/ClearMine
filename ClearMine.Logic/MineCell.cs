namespace ClearMine.Logic
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using ClearMine.Common.ComponentModel;

    internal class MineCell : BindableObject, IUpdatable<MineCell>
    {
        private CellState state;
        private int minesNearBy;
        private bool showResult;
        private bool hasMine;
        private bool isTerminator;

        internal MineCell(int column, int row)
        {
            UpdatePosition(column, row);
            State = CellState.Normal;
        }

        public event EventHandler StateChanged;

        [Bindable(true)]
        public bool HasMine
        {
            get { return hasMine; }
            set { SetProperty(ref hasMine, value); }
        }

        public int Row { get; private set; }

        public int Column { get; private set; }

        [Bindable(true)]
        public bool ShowResult
        {
            get { return showResult; }
            set { SetProperty(ref showResult, value, "ShowResult"); }
        }

        [Bindable(true)]
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

        [Bindable(true)]
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

        [Bindable(true)]
        public bool IsTerminator
        {
            get { return isTerminator; }
            set { SetProperty(ref isTerminator, value); }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "({0}, {1}) {3} : {2}", Column, Row, MinesNearby, HasMine);
        }

        internal void UpdatePosition(int column, int row)
        {
            Row = row;
            Column = column;
        }

        public void Update(MineCell newValue)
        {
            Row = newValue.Row;
            Column = newValue.Column;
        }

        internal bool Near(MineCell other)
        {
            return Math.Abs(Column - other.Column) <= 1 && Math.Abs(Row - other.Row) <= 1;
        }
    }
}
