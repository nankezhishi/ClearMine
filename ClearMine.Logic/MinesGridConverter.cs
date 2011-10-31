namespace ClearMine.Logic
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows;

    using ClearMine.Common.ComponentModel;
    using ClearMine.GameDefinition;

    /// <summary>
    /// Save MinesGrid data into string to save space.
    /// </summary>
    internal class MinesGridConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null || !(value is string))
            {
                return null;
            }

            var str = value as string;
            var grid = new MinesGrid();
            var rows = str.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int rowNum = 0;
            int columnNum = 0;
            foreach (var rowStr in rows)
            {
                columnNum = 0;
                var cells = rowStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cellstr in cells)
                {
                    var cell = new MineCell(columnNum, rowNum);
                    int cellState;
                    if (Int32.TryParse(cellstr.Substring(0, 1), out cellState))
                    {
                        cell.State = (CellState)cellState;
                    }
                    else
                    {
                        cell.State = CellState.Normal;
                    }

                    cell.HasMine = cellstr[1] == 'T' ? true : false;
                    grid.Add(cell);

                    ++columnNum;
                }

                ++rowNum;
            }

            grid.Size = new Size(columnNum, rowNum);

            return grid;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null || !(value is MinesGrid))
            {
                return null;
            }

            var minesGrid = value as MinesGrid;
            var strBuilder = new StringBuilder(minesGrid.Where(cell => cell.CachingState == CachingState.InUse).Count() * 4);

            foreach (var cell in minesGrid.Where(c => c.CachingState == CachingState.InUse))
            {
                strBuilder.Append((int)cell.State);
                strBuilder.Append(cell.HasMine ? 'T' : 'F');
                if (cell.Column == minesGrid.Size.Width - 1)
                {
                    strBuilder.Append('\n');
                }
                else
                {
                    strBuilder.Append(';');
                }
            }

            return strBuilder.ToString();
        }
    }
}
