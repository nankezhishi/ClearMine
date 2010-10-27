namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Windows;

    internal class MinesGrid : ObservableCollection<MineCell>
    {
        private Size size = Size.Empty;

        public MinesGrid()
        {
        }

        public MinesGrid(Size size)
            //: base (size.Width * size.Height)
        {
            SetSize(size);
        }

        public Size Size { get { return size; } }

        public void SetSize(Size newSize)
        {
            if (this.size != newSize)
            {
                this.size = newSize;

                // No need to clear the items.
                // Clear items is a heavy operaion. It could be a cache.

                // Generate cells row by row. So first Height then Width;
                for (int row = 0; row < newSize.Height; ++row)
                {
                    for (int column = 0; column < newSize.Width; ++column)
                    {
                        int index = row * (int)newSize.Width + column;
                        if (index < Count)
                        {
                            this[index].UpdatePosition(column, row);
                        }
                        else
                        {
                            base.Add(new MineCell(column, row));
                        }
                    }
                }

                // Don't worry about the redundent items.
                // UI Layer take the resposiblity of not showing them.
                // But we still need to clear mines on any initialize.

                OnPropertyChanged(new PropertyChangedEventArgs("Size"));
            }
        }

        public void MarkAllAsNoraml()
        {
            // All the cell, including the hidden ones must be updated.
            foreach (var cell in this)
            {
                cell.State = CellState.Normal;
            }
        }

        public void ClearMines()
        {
            // All the cell, including the hidden ones must be updated.
            foreach (var cell in this)
            {
                cell.HasMine = false;
            }
        }

        public void MoveMine(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);
            Debug.Assert(cell.HasMine);

            var emptyCells = GetCellsAround(null, c => !c.HasMine);
            var moveTo = emptyCells.ElementAt(new Random().Next(emptyCells.Count()));

            Debug.Assert(moveTo != cell);

            cell.HasMine = false;
            moveTo.HasMine = true;

            cell.MinesNearby = GetMinesCountNearBy(cell);

            foreach (var nearByCell in GetCellsAround(cell).Union(GetCellsAround(moveTo)))
            {
                nearByCell.MinesNearby = GetMinesCountNearBy(nearByCell);
            }
        }

        protected override void InsertItem(int index, MineCell item)
        {
            if (index >= Size.Width * Size.Height)
            {
                throw new InvalidOperationException("No slot available for more mine cells.");
            }

            base.InsertItem(index, item);
        }

        public MineCell GetCell(int column, int row)
        {
            return this[GetIndex(column, row)];
        }

        public bool CheckIfAllMarked(IEnumerable<MineCell> cells)
        {
            return !(cells ?? GetCellsAround(null)).Any(c => c.HasMine && c.State != CellState.MarkAsMine);
        }

        public bool ContainsWrongMark(IEnumerable<MineCell> cells)
        {
            return (cells ?? GetCellsAround(null)).Any(c => !c.HasMine && c.State == CellState.MarkAsMine);
        }

        public void ShowAll()
        {
            foreach (var nearCell in GetCellsAround(null, cell => cell.State == CellState.Normal || cell.State == CellState.Question))
            {
                nearCell.State = CellState.Shown;
            }
        }

        public void ExpandFrom(MineCell current)
        {
            Contract.Requires<ArgumentNullException>(current != null);

            if (current.MinesNearby == 0)
            {
                foreach (var nearCell in GetCellsAround(current, cell => cell.State == CellState.Normal || cell.State == CellState.Question))
                {
                    nearCell.State = CellState.Shown;
                    ExpandFrom(nearCell);
                }
            }
        }

        public bool TryExpandFrom(MineCell current)
        {
            Contract.Requires<ArgumentNullException>(current != null);

            if (current.State == CellState.Shown)
            {
                var cellsNearBy = GetCellsAround(current, cell => cell.State == CellState.Normal || cell.State == CellState.Question);

                if (CheckIfAllMarked(cellsNearBy))
                {
                    foreach (var cell in cellsNearBy)
                    {
                        cell.State = CellState.Shown;
                        if (cell.MinesNearby == 0)
                        {
                            ExpandFrom(cell);
                        }
                    }

                    return true;
                }
                else if (ContainsWrongMark(cellsNearBy))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<MineCell> GetCellsAround(MineCell current, Predicate<MineCell> condition = null)
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
                    if ((condition ?? (cell => true))(result))
                    {
                        yield return result;
                    }
                }
            }
        }

        public int GetIndex(int column, int row)
        {
            Verify(column, row);

            return row * (int)size.Width + column;
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

        internal int GetMinesCountNearBy(MineCell cell)
        {
            Contract.Requires<ArgumentNullException>(cell != null);

            return GetCellsAround(cell, c => c.HasMine).Count();
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
