namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows;
    using ClearMine.Framework;

    internal class ClearMineGame : BindableObject, IClearMine
    {
        private MinesGrid cells = new MinesGrid();
        private GameState gameState;
        private int totalMines;
        private int remainedMines;
        private Stopwatch watch;

        public event EventHandler StateChanged;

        public void Initialize(Size size, int mines)
        {
            //TODO: Check size and mines.

            this.watch = null;
            this.remainedMines = 0;
            this.totalMines = mines;
            this.cells.SetSize(size);
            StartNew();
        }

        public void Restart()
        {
            this.GameState = GameState.Initialized;
            this.cells.MarkAllAsNoraml();
        }

        public void StartNew()
        {
            Restart();
            this.cells.ClearMines();
            new MinesGenerator().Fill(ref this.cells, totalMines);
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
            if (watch != null && cells.CheckIfAllMarked(cells))
            {
                cells.ShowAll();
                watch.Stop();
                GameState = GameState.Success;
            }
        }

        public GameState TryDigAt(MineCell cell)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);

            if (cell.HasMine)
            {
                // Hit the mine on first hit.
                if (watch == null)
                {
                    this.cells.MoveMine(cell);
                    cell.State = CellState.Shown;
                    cells.ExpandFrom(cell);
                }
                else
                {
                    watch.Stop();
                    GameState = GameState.Failed;
                }
            }
            else
            {
                cell.State = CellState.Shown;
                cells.ExpandFrom(cell);
            }

            if (watch == null)
            {
                watch = Stopwatch.StartNew();
                GameState = GameState.Started;
            }

            return GameState;
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

        public Size Size
        {
            get { return cells.Size; }
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
