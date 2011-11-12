namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.GameDefinition;

    [Serializable]
    [TypeConverter(typeof(MinesGridConverter))]
    internal class MinesGrid : CachedObservableCollection<MineCell>
    {
        private Size size = Size.Empty;

        internal Size Size
        {
            get { return size; }
            set { this.size = value; }
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder((int)(Size.Width + Environment.NewLine.Length) * (int)Size.Height);
            for (int row = 0; row < Size.Height; ++row)
            {
                for (int column = 0; column < Size.Width; ++column)
                {
                    var cell = GetCell(column, row);
                    strBuilder.Append(cell.HasMine ? '*' : cell.MinesNearby);
                }

                strBuilder.Append(Environment.NewLine);
            }

            return strBuilder.ToString();
        }

        internal IEnumerable<MineCell> SetSize(Size newSize)
        {
            if (this.size != newSize)
            {
                this.size = newSize;
                this.Clear();
                // Generate cells row by row. So first Height then Width;
                int index = 0;
                for (int row = 0; row < newSize.Height; ++row)
                {
                    for (int column = 0; column < newSize.Width; ++column)
                    {
                        var newCell = new MineCell(column, row);

                        // if call base.InsertItem directly, C# compiler will unable to generate a valid assembly!
                        // Woops.
                        this.InsertItem(index++, newCell);
                        if (newCell.CachingState == CachingState.InUse)
                        {
                            yield return newCell;
                        }
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Size"));
            }
        }

        internal void MarkAllAsNoraml()
        {
            // All the cell, including the hidden ones must be updated.
            foreach (var cell in this)
            {
                cell.ShowResult = false;
                cell.IsTerminator = false;
                cell.State = CellState.Normal;
            }
        }

        internal void ClearMines()
        {
            // All the cell, including the hidden ones must be updated.
            foreach (var cell in this)
            {
                cell.HasMine = false;
            }
        }

        /// <summary>
        /// Make a cell clear by moving mines nearby to other places.
        /// </summary>
        /// <param name="cell"></param>
        internal void ClearMineAround(MineCell cell)
        {
            var updateList = new List<MineCell>();

            foreach (MineCell cellHasMine in GetCellsAround(cell, null).Union(new[] { cell }).Where(c => c.HasMine))
            {
                var emptyCells = GetCellsAround(null, c => !(c.HasMine || c.Near(cell)));
                var moveTo = emptyCells.ElementAt(new Random().Next(emptyCells.Count()));

                cellHasMine.HasMine = false;
                moveTo.HasMine = true;

                updateList.Add(cellHasMine);
                updateList.Add(moveTo);
            }

            var calculateList = new List<MineCell>();
            foreach (var updatedCell in updateList)
            {
                calculateList.AddRange(GetCellsAround(updatedCell));
            }

            CalculateMinesCount(calculateList.Union(updateList).Distinct());
        }

        internal void CalculateMinesCount(IEnumerable<MineCell> cells = null)
        {
            if (cells == null)
            {
                cells = GetCellsAround(null, c => !c.HasMine);
            }

            Parallel.ForEach(cells, c => c.MinesNearby = GetCellsAround(c, s => s.HasMine).Count());
        }

        internal void CalculateFlagsCount(IEnumerable<MineCell> cells = null)
        {
            if (cells == null)
            {
                cells = GetCellsAround(null, c => c.State == CellState.Shown);
            }

            if (Settings.Default.ShowTooManyFlagsWarning)
            {
                Parallel.ForEach(cells, c => c.FlagsNearBy = GetCellsAround(c, s => s.State == CellState.MarkAsMine).Count());
            }
            else
            {
                Parallel.ForEach(cells, c => c.FlagsNearBy = 0);
            }
        }

        internal MineCell GetCell(int column, int row)
        {
            return this[GetIndex(column, row)];
        }

        internal bool CheckWinning()
        {
            // All the un-shown items has mine. Means Win.
            return GetCellsAround(null, c => c.State != CellState.Shown).All(c => c.HasMine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns>All the cells that Expanded.</returns>
        internal IEnumerable<MineCell> ExpandFrom(MineCell current)
        {
            if (current.State != CellState.Shown)
            {
                current.State = CellState.Shown;
                yield return current;
                if (current.MinesNearby == 0)
                {
                    foreach (var nearCell in GetCellsAround(current, cell =>
                        cell.State == CellState.Normal || cell.State == CellState.Question))
                    {
                        // Though we get cells that only Normal or Question.
                        // We still need to check it. 
                        if (nearCell.State == CellState.Shown)
                        {
                            continue;
                        }

                        foreach (var cell in ExpandFrom(nearCell))
                        {
                            if (cell != current)
                            {
                                yield return cell;
                            }
                        }
                    }
                }
                else
                {
                    // Do nothing.
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns>The number of cells that has no mines around from the current mine cell.</returns>
        internal IEnumerable<MineCell> TryExpandFrom(MineCell current)
        {
            if (current.State == CellState.Shown)
            {
                var cellsNearBy = GetCellsAround(current, cell => cell.State != CellState.Shown);

                if (CheckIfAllMarked(cellsNearBy))
                {
                    foreach (var cell in cellsNearBy.Where(c => c.State != CellState.MarkAsMine))
                    {
                        foreach (var expanded in ExpandFrom(cell))
                        {
                            yield return expanded;
                        }
                    }
                }
                else if (ContainsWrongMark(cellsNearBy))
                {
                    throw new ExpandFailedException();
                }
                else
                {
                    PressCellsAround(current);
                }
            }
            else
            {
                PressCellsAround(current);
            }
        }

        internal IEnumerable<MineCell> GetCellsAround(MineCell current, Predicate<MineCell> condition = null)
        {
            int startColumn = 0;
            int endColumn = (int)Size.Width - 1;
            int startRow = 0;
            int endRow = (int)Size.Height - 1;

            if (current != null)
            {
                startColumn = current.Column - 1;
                endColumn = current.Column + 1;
                startRow = current.Row - 1;
                endRow = current.Row + 1;
            }

            for (int i = startColumn; i <= endColumn; ++i)
            {
                if (i < 0 || i >= Size.Width)
                {
                    continue;
                }

                for (int j = startRow; j <= endRow; ++j)
                {
                    if (j < 0 || j >= Size.Height)
                    {
                        continue;
                    }

                    if (current != null && (i == current.Column && j == current.Row) /*Exclude it self*/)
                    {
                        continue;
                    }

                    var result = GetCell(i, j);
                    if (condition == null || condition(result))
                    {
                        yield return result;
                    }
                }
            }
        }

        internal int GetIndex(int column, int row)
        {
            Verify(column, row);

            return row * (int)size.Width + column;
        }

        internal void DoForThat(Predicate<MineCell> condition, Action<MineCell> action)
        {
            foreach (var nearCell in GetCellsAround(null, condition))
            {
                action(nearCell);
            }
        }

        private void PressCellsAround(MineCell current)
        {
            foreach (var cell in GetCellsAround(current, c => c.State == CellState.Normal || c.State == CellState.Question))
            {
                cell.PressState = PressState.Pressed;
            }
        }

        protected override void InsertItem(int index, MineCell item)
        {
            if (index >= Size.Width * Size.Height)
            {
                throw new InvalidOperationException(LocalizationHelper.FindText("MinesGridOverflow"));
            }

            base.InsertItem(index, item);
        }

        private bool ContainsWrongMark(IEnumerable<MineCell> cells)
        {
            return (cells ?? GetCellsAround(null)).Any(c => !c.HasMine && c.State == CellState.MarkAsMine);
        }

        private bool CheckIfAllMarked(IEnumerable<MineCell> cells)
        {
            return !(cells ?? GetCellsAround(null)).Any(c => c.HasMine && c.State != CellState.MarkAsMine);
        }

        private void Verify(int column, int row)
        {
            if (column >= Size.Width || column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }

            if (row >= Size.Height || row < 0)
            {
                throw new ArgumentOutOfRangeException("row");
            }
        }
    }
}
