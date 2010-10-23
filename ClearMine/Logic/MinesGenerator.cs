namespace ClearMine.Logic
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal class MinesGenerator
    {
        private Random random = new Random();

        public void Fill(ref MinesGrid grid, int mines, MineCell noMineCell = null)
        {
            Contract.Requires<ArgumentNullException>(grid != null);

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
