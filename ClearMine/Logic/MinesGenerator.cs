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

            int total = grid.Width * grid.Height;
            grid.Clear();

            for (int i = 0; i < grid.Width; ++i)
            {
                for (int j = 0; j < grid.Height; ++j)
                {
                    grid.Add(new MineCell(i, j));
                }
            }

            while (mines > 0)
            {
                int index = random.Next(total);
                if (!grid[index].HasMine)
                {
                    grid[index].HasMine = true;
                    --mines;
                }
            }

            for (int i = 0; i < grid.Width; ++i)
            {
                for (int j = 0; j < grid.Height; ++j)
                {
                    grid.GetCell(i, j).MinesNearby = grid.GetCellsAround(i, j, cell => cell.HasMine).Count();
                }
            }

            return grid;
        }
    }
}
