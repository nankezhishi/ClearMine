namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using ClearMine.Framework.ComponentModel;

    internal class ClearMineGame : BindableObject, IClearMine
    {
        private int totalMines;
        private Stopwatch watch;
        private GameState gameState;
        private DispatcherTimer timer;
        private MinesGrid cells = new MinesGrid();

        public event EventHandler StateChanged;
        public event EventHandler TimeChanged;

        public ClearMineGame()
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
            timer.Tick += new EventHandler(OnTick);
        }

        public void Initialize(Size size, int mines)
        {
            this.totalMines = mines;
            this.cells.SetSize(size);
            StartNew();
        }

        public void Restart()
        {
            this.timer.Stop();
            this.watch = null;
            this.GameState = GameState.Initialized;
            this.cells.MarkAllAsNoraml();
            OnTick(this, EventArgs.Empty);
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
                this.watch.Stop();
                this.timer.Stop();
                this.cells.ShowAll();
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
                    this.cells.ExpandFrom(cell);
                }
                else
                {
                    this.watch.Stop();
                    this.timer.Stop();
                    GameState = GameState.Failed;
                }
            }
            else
            {
                cell.State = CellState.Shown;
                this.cells.ExpandFrom(cell);
            }

            if (watch == null)
            {
                this.timer.Start();
                this.watch = Stopwatch.StartNew();
                this.GameState = GameState.Started;
            }

            return GameState;
        }

        public MineCell GetCell(int column, int row)
        {
            return this.cells.GetCell(column, row);
        }

        public int UsedTime
        {
            get { return this.watch == null ? 0 : (int)watch.ElapsedMilliseconds; }
        }

        public int TotalMines
        {
            get { return totalMines; }
        }

        public int RemainedMines
        {
            get
            {
                return totalMines - (from cell in cells
                                     where cell.State == CellState.MarkAsMine
                                     select cell).Count();
            }
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

        private void OnTick(object sender, EventArgs e)
        {
            OnPropertyChanged("UsedTime");
            var temp = TimeChanged;
            if (temp != null)
            {
                temp(this, e);
            }
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
