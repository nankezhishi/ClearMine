namespace ClearMine.Logic
{
    using System;
    using System.Diagnostics.Contracts;

    internal class MinesGenerator
    {
        private Random random = new Random();

        public void Fill(ref MinesGrid grid, int mines, MineCell noMineCell = null)
        {
            Contract.Requires<ArgumentNullException>(grid != null);
            if (mines > grid.Size.Height * grid.Size.Width * 0.4)
            {
                throw new InvalidOperationException("Too many mines to generate.");
            }

            int total = (int)(grid.Size.Width * grid.Size.Height);

            while (mines > 0)
            {
                int index = random.Next(total);
                if (!grid[index].HasMine && grid[index] != noMineCell)
                {
                    grid[index].HasMine = true;
                    --mines;
                }
            }

            foreach(var cell in grid)
            {
                cell.MinesNearby = grid.GetMinesCountNearBy(cell);
            }
        }
    }
}
