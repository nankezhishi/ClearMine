namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    using ClearMine.Common.ComponentModel;

    [Serializable]
    [XmlRoot("savedGame")]
    public class ClearMineGame : BindableObject, IClearMine
    {
        private int totalMines;
        private int? hash;
        private int usedTime;
        private DateTime startTime;
        private GameState gameState;
        private DispatcherTimer timer;
        private MinesGrid cells = new MinesGrid();

        [field: NonSerialized]
        public event EventHandler StateChanged;
        [field: NonSerialized]
        public event EventHandler TimeChanged;
        [field: NonSerialized]
        public event EventHandler<CellStateChangedEventArgs> CellStateChanged;

        public ClearMineGame()
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            timer.Tick += new EventHandler(OnTick);
        }

        [XmlAttribute("timeUsed")]
        public int UsedTime
        {
            get
            {
                if (timer.IsEnabled)
                {
                    return (int)(DateTime.Now - startTime).TotalMilliseconds;
                }
                else
                {
                    return usedTime;
                }
            }
            // setter is a must for XML Serialization.
            set { PersistantUsedTime(value); }
        }

        [XmlIgnore]
        public int TotalMines
        {
            get { return totalMines; }
        }

        [XmlIgnore]
        public int RemainedMines
        {
            get { return totalMines - cells.Count(cell => cell.State == CellState.MarkAsMine); }
        }

        [XmlIgnore]
        public GameState GameState
        {
            get { return gameState; }
            private set
            {
                if (SetProperty(ref gameState, value, "GameState"))
                {
                    if (value == GameState.Success || value == GameState.Failed)
                    {
                        PersistantUsedTime(UsedTime);
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
                        usedTime = 0;
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

        [XmlIgnore]
        public IEnumerable<MineCell> Cells
        {
            get { return cells; }
        }

        [XmlAttribute("hash")]
        public int Hash
        {
            get
            {
                if (!hash.HasValue)
                {
                    hash = CellsForSerialize.GetHashCode() ^ UsedTime.GetHashCode();
                }

                return hash.Value;
            }
            set { hash = value; }
        }

        [XmlText]
        public string CellsForSerialize
        {
            get
            {
                if (cells == null)
                {
                    return null;
                }

                var gridConverter = TypeDescriptor.GetConverter(typeof(MinesGrid));
                return gridConverter.ConvertToInvariantString(cells);
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    var gridConverter = TypeDescriptor.GetConverter(typeof(MinesGrid));
                    cells = (MinesGrid)gridConverter.ConvertFromInvariantString(value);
                    cells.CalculateMinesCount();
                    totalMines = cells.Where(c => c.HasMine).Count();
                    gameState = GameState.Started;
                }
            }
        }

        [XmlIgnore]
        public Size Size
        {
            get { return cells.Size; }
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
            this.cells.CalculateMinesCount();
        }

        public bool CheckHash()
        {
            return Hash == (CellsForSerialize.GetHashCode() ^ UsedTime.GetHashCode());
        }

        public void Update(IClearMine mine)
        {
            var game = mine as ClearMineGame;

            if (game == null)
            {
                throw new ArgumentException("mine");
            }

            int index = 0;
            
            this.cells.Clear();
            this.cells.Size = game.cells.Size;
            foreach (var cell in game.Cells)
            {
                this.cells.Insert(index++, cell);
                if (cell.CachingState == CachingState.InUse)
                {
                    cell.StateChanged += new EventHandler<CellStateChangedEventArgs>(OnCellStateChanged);
                }
            }

            totalMines = game.totalMines;
            PersistantUsedTime(game.UsedTime);
            GameState = game.GameState;
        }

        public void Pause()
        {
            timer.IsEnabled = false;
            usedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
        }

        public void Resume()
        {
            timer.IsEnabled = true;
            PersistantUsedTime(usedTime);
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
            if (cell.HasMine && cell.State != CellState.MarkAsMine)
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

        private void PersistantUsedTime(int usedTime)
        {
            Debug.Assert(timer.IsEnabled);

            this.usedTime = usedTime;
            startTime = DateTime.Now.AddMilliseconds(-usedTime);
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
