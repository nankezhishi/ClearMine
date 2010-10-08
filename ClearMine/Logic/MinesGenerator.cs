namespace ClearMine.Logic
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal class MinesGenerator
    {
        private Random random = new Random();

        public MinesGrid Fill(ref MinesGrid grid, int mines)
        {
            Contract.Requires<ArgumentNullException>(grid != null);

            grid.Clear();

            // Generate cells row by row. So first Height then Width;
            for (int row = 0; row < grid.Height; ++row)
            {
                for (int column = 0; column < grid.Width; ++column)
                {
                    grid.Add(new MineCell(column, row));
                }
            }

            int total = grid.Width * grid.Height;
            while (mines > 0)
            {
                int index = random.Next(total);
                if (!grid[index].HasMine)
                {
                    grid[index].HasMine = true;
                    --mines;
                }
            }

            foreach(var cell in grid)
            {
                cell.MinesNearby = grid.GetCellsAround(cell, c => c.HasMine).Count();
            }

            return grid;
        }
    }
}
