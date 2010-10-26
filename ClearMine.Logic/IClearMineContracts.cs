namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;

    [ContractClassFor(typeof(IClearMine))]
    internal abstract class IClearMineContracts : IClearMine
    {
        public void Initialize(Size size, int mines)
        {
        }

        public void Restart()
        {
        }

        public void StartNew()
        {
        }

        public void TryMarkAt(MineCell cell, CellState newState)
        {
            Contract.Requires<ArgumentNullException>(cell != null);
        }

        public GameState TryDigAt(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);

            return default(GameState);
        }

        public bool TryExpandAt(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);

            return default(bool);
        }

        public MineCell GetCell(int column, int row)
        {
            throw new NotImplementedException();
        }

        public int UsedTime
        {
            get { throw new NotImplementedException(); }
        }

        public int TotalMines
        {
            get { throw new NotImplementedException(); }
        }

        public int RemainedMines
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Size Size
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<MineCell> Cells
        {
            get { throw new NotImplementedException(); }
        }

        public GameState GameState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler StateChanged;

        public event EventHandler TimeChanged;
    }
}
