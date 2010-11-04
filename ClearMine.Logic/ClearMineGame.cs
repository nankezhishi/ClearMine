namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    using ClearMine.Common.ComponentModel;

    internal class ClearMineGame : BindableObject, IClearMine
    {
        private int totalMines;
        private DateTime startTime;
        private GameState gameState;
        private DispatcherTimer timer;
        private MinesGrid cells = new MinesGrid();

        public event EventHandler StateChanged;
        public event EventHandler TimeChanged;
        public event EventHandler<CellStateChangedEventArgs> CellStateChanged;

        public ClearMineGame()
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            timer.Tick += new EventHandler(OnTick);
        }

        public void Initialize(Size size, int mines)
        {
            this.totalMines = mines;
            foreach (var newCell in this.cells.SetSize(size))
            {
                newCell.StateChanged += new EventHandler<CellStateChangedEventArgs>(OnCellStateChanged);
            }
        }

        public void Restart()
        {
            this.GameState = GameState.Initialized;
            OnTick(this, EventArgs.Empty);
        }

        public void StartNew()
        {
            Restart();
            this.cells.ClearMines();
            new MinesGenerator().Fill(this.cells, totalMines);
        }

        public void Pause()
        {
            timer.IsEnabled = false;
        }

        public void Resume()
        {
            timer.IsEnabled = true;
        }

        public IEnumerable<MineCell> TryExpandAt(MineCell cell)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);

            try
            {
                var result = cells.TryExpandFrom(cell).ToList();
                if (cells.CheckWinning())
                {
                    GameState = GameState.Success;
                }

                return result;
            }
            catch (ExpandFailedException)
            {
                GameState = GameState.Failed;
                return new List<MineCell>();
            }
        }

        public void TryMarkAt(MineCell cell, CellState newState)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);
            if (!new[] { CellState.MarkAsMine, CellState.Normal, CellState.Question }.Contains(newState))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Cannot set cell state to {0}.", newState));
            }

            cell.State = newState;
            if (cells.CheckWinning())
            {
                GameState = GameState.Success;
            }
        }

        public IEnumerable<MineCell> TryDigAt(MineCell cell)
        {
            VerifyStateIs(GameState.Initialized, GameState.Started);

            var result = new List<MineCell>();

            // Hit the mine on first hit.
            if (GameState == GameState.Initialized)
            {
                this.cells.ClearMineAround(cell);
                GameState = GameState.Started;
                result = this.cells.ExpandFrom(cell).ToList();
                if (cells.CheckWinning())
                {
                    GameState = GameState.Success;
                }
            }

            // Change Game State Once! Don't change it more than one time.
            if (cell.HasMine)
            {
                cell.IsTerminator = true;
                GameState = GameState.Failed;
            }
            else if (cell.State == CellState.Normal || cell.State == CellState.Question)
            {
                result = this.cells.ExpandFrom(cell).ToList();
                if (cells.CheckWinning())
                {
                    GameState = GameState.Success;
                }
                else if (GameState == GameState.Initialized)
                {
                    GameState = GameState.Started;
                }
            }

            return result;
        }

        public MineCell GetCell(int column, int row)
        {
            return this.cells.GetCell(column, row);
        }

        public int UsedTime
        {
            get
            {
                if (timer.IsEnabled)
                {
                    return (int)(DateTime.Now - startTime).TotalMilliseconds;
                }

                return 0;
            }
        }

        public int TotalMines
        {
            get { return totalMines; }
        }

        public int RemainedMines
        {
            get { return totalMines - cells.Count(cell => cell.State == CellState.MarkAsMine); }
        }

        public GameState GameState
        {
            get { return gameState; }
            private set
            {
                if (SetProperty(ref gameState, value))
                {
                    if (value == GameState.Success || value == GameState.Failed)
                    {
                        this.timer.Stop();
                        this.cells.DoForThat(c => c.HasMine || c.State == CellState.MarkAsMine, c => c.ShowResult = true);
                    }
                    else if (value == GameState.Started)
                    {
                        this.timer.Start();
                        this.startTime = DateTime.Now;
                    }
                    else if (value == GameState.Initialized)
                    {
                        this.timer.Stop();
                        this.cells.MarkAllAsNoraml();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    if (StateChanged != null)
                    {
                        StateChanged.Invoke(this, EventArgs.Empty);
                    }
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

        private void OnCellStateChanged(object sender, CellStateChangedEventArgs e)
        {
            var cell = sender as MineCell;
            if (cell != null && cell.CachingState == CachingState.InUse)
            {
                var temp = CellStateChanged;
                if (temp != null)
                {
                    temp(this, e);
                }
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
