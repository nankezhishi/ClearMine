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

        public void Pause()
        {

        }

        public void Resume()
        {

        }

        public void Update(IClearMine clearMine)
        {
            Contract.Requires<ArgumentNullException>(clearMine != null);
        }

        public void TryMarkAt(MineCell cell, CellState newState)
        {
            Contract.Requires<ArgumentNullException>(cell != null);
        }

        public IEnumerable<MineCell> TryDigAt(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);

            return default(IEnumerable<MineCell>);
        }

        public IEnumerable<MineCell> TryExpandAt(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);

            return default(IEnumerable<MineCell>);
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

        public Size Size
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

        public event EventHandler<CellStateChangedEventArgs> CellStateChanged;
    }
}
