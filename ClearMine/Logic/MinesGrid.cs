namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    internal class MinesGrid : ObservableCollection<MineCell>
    {
        private int width = -1;
        private int height = -1;

        public MinesGrid()
            : this(0, 0)
        {
        }

        public MinesGrid(int width, int height)
            //: base (width * height)
        {
            SetSize(width, height);
        }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public void SetSize(int width, int height)
        {
            Contract.Requires<ArgumentOutOfRangeException>(width >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(height >= 0);

            if (this.width != width)
            {
                this.width = width;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
            if (this.height != height)
            {
                this.height = height;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        protected override void InsertItem(int index, MineCell item)
        {
            if (index >= width * height)
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
            foreach (var cell in cells ?? this)
            {
                if (cell.HasMine && cell.State != CellState.MarkAsMine)
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainsWrongMark(IEnumerable<MineCell> cells)
        {
            foreach (var cell in cells ?? this)
            {
                if (!cell.HasMine && cell.State == CellState.MarkAsMine)
                {
                    return true;
                }
            }

            return false;
        }

        public void ExpandFrom(MineCell currentCell)
        {
            Contract.Requires<ArgumentNullException>(currentCell != null);

            if (currentCell.MinesNearby == 0)
            {
                foreach (var nearCell in GetCellsAround(currentCell.Column, currentCell.Row, cell => cell.State == CellState.Normal || cell.State == CellState.Question))
                {
                    nearCell.State = CellState.Shown;
                    ExpandFrom(currentCell);
                }
            }
        }

        public bool TryExpandFrom(MineCell currentCell)
        {
            Contract.Requires<ArgumentNullException>(currentCell != null);

            if (currentCell.State == CellState.Shown)
            {
                var cellsNearBy = GetCellsAround(currentCell.Column, currentCell.Row, cell => cell.State == CellState.Normal || cell.State == CellState.Question);

                if (CheckIfAllMarked(cellsNearBy))
                {
                    foreach (var cell in cellsNearBy)
                    {
                        cell.State = CellState.Shown;
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

        public IEnumerable<MineCell> GetCellsAround(int column, int row, Predicate<MineCell> condition)
        {
            Verify(column, row);

            for (int i = column - 1; i <= column + 1; ++i)
            {
                if (i < 0 || i >= column)
                {
                    continue;
                }

                for (int j = row - 1; j <= row + 1; ++j)
                {
                    if (j < 0 || j >= height || (i == column && j == row) /*Exclude it self*/)
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

            return row * width + column;
        }

        private void Verify(int column, int row)
        {
            if (width < 0 || height < 0)
            {
                throw new InvalidOperationException("A mine grid must be set to a valid size before using it.");
            }

            if (column >= width || column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }

            if (row >= height || row < 0)
            {
                throw new ArgumentOutOfRangeException("row");
            }
        }
    }
}
