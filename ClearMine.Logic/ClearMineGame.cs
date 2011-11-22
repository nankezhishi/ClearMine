namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml.Serialization;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Messages;

    [Serializable]
    [XmlRoot("savedGame")]
    [Export(typeof(IClearMine))]
    public class ClearMineGame : BindableObject, IClearMine
    {
        private int totalMines;
        private int? hash;
        private int usedTime;
        private DateTime startTime;
        private GameState gameState;
        [NonSerialized]
        private DispatcherTimer timer;
        private MinesGrid cells = new MinesGrid();

        public ClearMineGame()
        {
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            timer.Tick += new EventHandler(OnTick);
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(OnSettingsChanged);
            MessageManager.SubscribeMessage<CellStateMessage>(OnCellStatusChanged);
        }

        [field: NonSerialized]
        public event EventHandler TimeChanged;

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
                var oldState = GameState;

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
                        // Resume a paused game;
                        if (oldState == GameState.Paused)
                        {
                            timer.IsEnabled = true;
                            PersistantUsedTime(usedTime);
                        }
                        // start a new game.
                        else
                        {
                            this.timer.Start();
                            this.startTime = DateTime.Now;
                        }
                    }
                    else if (value == GameState.Initialized)
                    {
                        usedTime = 0;
                        this.timer.Stop();
                        this.cells.MarkAllAsNoraml();
                    }
                    else if (value == GameState.Paused)
                    {
                        timer.IsEnabled = false;
                        usedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    MessageManager.SendMessage<GameStateMessage>(this);
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
                    cells.CalculateFlagsCount(cells);
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
            this.cells.SetSize(size);
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
            var generator = Infrastructure.Container.GetExportedValue<IMinesGenerator>();
            if (generator != null)
            {
                generator.Fill(this.cells.Size, this.cells, totalMines);
            }
            else
            {
                throw new InvalidOperationException("Cannot find a proper mines generator.");
            }
            this.cells.CalculateMinesCount();
            this.cells.CalculateFlagsCount(this.cells);
        }

        public bool CheckHash()
        {
            return Hash == (CellsForSerialize.GetHashCode() ^ UsedTime.GetHashCode());
        }

        public MineCell GetCell(int column, int row)
        {
            return cells[row * (int)Size.Width + column];
        }

        public void Update(IClearMine newValue)
        {
            var game = newValue as ClearMineGame;

            if (game == null)
            {
                throw new ArgumentException(ResourceHelper.FindText("InvalidUpdateGameType", GetType().Name), "newValue");
            }

            int index = 0;

            this.cells.Clear();
            this.cells.Size = game.cells.Size;
            foreach (var cell in game.Cells)
            {
                this.cells.Insert(index++, cell);
            }

            totalMines = game.totalMines;
            PersistantUsedTime(game.UsedTime);
            GameState = game.GameState;
        }

        public void PauseGame()
        {
            // Pause twice cause game time not accurate.
            if (timer.IsEnabled)
            {
                GameState = GameState.Paused;
            }
        }

        public void ResumeGame()
        {
            GameState = GameState.Started;
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
                MessageManager.SendMessage<UserOperationMessage>(this, GameOperation.Expand, result);

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
            if (cell == null)
                throw new ArgumentNullException("cell");

            VerifyStateIs(GameState.Initialized, GameState.Started);
            if (!new[] { CellState.MarkAsMine, CellState.Normal, CellState.Question }.Contains(newState))
            {
                throw new InvalidOperationException(ResourceHelper.FindText("InvalidTargetCellState", newState));
            }

            cell.State = newState;
            if (cells.CheckWinning())
            {
                GameState = GameState.Success;
            }

            MessageManager.SendMessage<UserOperationMessage>(this, GameOperation.MarkAs, new[] { cell });
        }

        public IEnumerable<MineCell> TryDigAt(MineCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("cell");

            VerifyStateIs(GameState.Initialized, GameState.Started);

            var result = new List<MineCell>();

            // Hit the mine on first hit.
            if (GameState == GameState.Initialized)
            {
                // Don't dig at a Marked cell.
                if (cell.State != CellState.MarkAsMine)
                {
                    this.cells.ClearMineAround(cell);
                    GameState = GameState.Started;
                    result = this.cells.ExpandFrom(cell).ToList();
                    if (cells.CheckWinning())
                    {
                        GameState = GameState.Success;
                    }
                }
                else
                {
                    // Acturally, the dig operation doesn't really performed, so no need to send dig message here.
                    return result;
                }
            }

            // Change Game State Once! Don't change it more than one time.
            if (cell.HasMine && cell.State != CellState.MarkAsMine)
            {
                cell.IsTerminator = true;
                GameState = GameState.Failed;
            }
            // Cannot dig at a flagged cell.
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
            MessageManager.SendMessage<UserOperationMessage>(this, GameOperation.Dig, result);

            return result;
        }

        private void PersistantUsedTime(int usedTime)
        {
            this.usedTime = usedTime;
            startTime = DateTime.Now.AddMilliseconds(-usedTime);
        }

        private void OnTick(object sender, EventArgs e)
        {
            TriggerPropertyChanged("UsedTime");
            var temp = TimeChanged;
            if (temp != null)
            {
                temp(this, e);
            }
        }

        private void OnCellStatusChanged(CellStateMessage message)
        {
            if (message.Cell.CachingState == CachingState.InUse &&
                GameState == GameState.Started)
            {
                cells.CalculateFlagsCount();
            }
        }

        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("ShowTooManyFlagsWarning".Equals(e.PropertyName))
            {
                cells.CalculateFlagsCount();
            }
        }

        private void VerifyStateIs(params GameState[] args)
        {
            if (!args.Contains(GameState))
            {
                throw new InvalidOperationException();
            }
        }
    }
}
