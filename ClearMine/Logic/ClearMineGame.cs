namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using ClearMine.Framework;

    internal class ClearMineGame : BindableObject, IClearMine
    {
        private MinesGrid cells = new MinesGrid();
        private GameState gameState;
        private int totalMines;
        private int remainedMines;
        private Stopwatch watch;

        public event EventHandler StateChanged;

        public void Initialize(int width, int height, int mines)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentOutOfRangeException("width", "The playground cannot be zero in size");
            }
            if (mines >= width * height)
            {
                throw new ArgumentOutOfRangeException("mines", "The mines count is too large.");
            }
            if (width * height > 1 << 10)
            {
                throw new NotSupportedException("The size of the playground is too large");
            }

            this.watch = null;
            this.remainedMines = 0;
            this.totalMines = mines;
            this.cells.SetSize(width, height);
            StartNew();
        }

        public void StartNew()
        {
            new MinesGenerator().Fill(ref this.cells, totalMines);
            this.GameState = GameState.Initialized;
        }

        public bool TryExpandAt(MineCell cell)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);

            return cells.TryExpandFrom(cell);
        }

        public void TryMarkAt(MineCell cell, CellState newState)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);

            cell.State = newState;
            if (cells.CheckIfAllMarked(cells))
            {
                watch.Stop();
                GameState = GameState.Success;
            }
        }

        public bool TryDigAt(MineCell cell)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);
            if (watch == null)
            {
                watch = Stopwatch.StartNew();
                GameState = GameState.Started;
            }

            if (cell.HasMine)
            {
                watch.Stop();
                GameState = GameState.Failed;
                return false;
            }
            else
            {
                cell.State = CellState.Shown;
                cells.ExpandFrom(cell);
                return true;
            }
        }

        public MineCell GetCell(int column, int row)
        {
            return cells.GetCell(column, row);
        }

        public int UsedTime
        {
            get { return watch == null ? 0 : (int)watch.ElapsedMilliseconds / 1000; }
        }

        public int TotalMines
        {
            get { return totalMines; }
        }

        public int RemainedMines
        {
            get { return remainedMines; }
        }

        public GameState GameState
        {
            get { return gameState; }
            set
            {
                if (SetProperty(ref gameState, value) && StateChanged != null)
                {
                    StateChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public IEnumerable<MineCell> Cells
        {
            get { return cells; }
        }

        private void VerifyStateIs(params GameState[] args)
        {
            Contract.Requires<ArgumentNullException>(args != null);

            if (!args.Contains(GameState))
            {
                throw new InvalidOperationException();
            }
        }
    }
}
