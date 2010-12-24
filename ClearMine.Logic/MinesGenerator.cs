namespace ClearMine.Logic
{
    using System;

    using ClearMine.Common.Logic;

    internal class MinesGenerator
    {
        private Random random = new Random();

        public void Fill(MinesGrid grid, int mines)
        {
            int total = (int)(grid.Size.Width * grid.Size.Height);

            if (mines > total)
            {
                mines = total;
            }

            // Choose different algorithm according to the mines to fill.
            // While, 0.5 is a guess.
            if (mines > grid.Size.Height * grid.Size.Width * 0.5)
            {
                #region A MUTATION of Reservoir Sampling Algorithm
                ///////////////////////////////////////////////////////////////////////////////
                //               Reservoir Sampling Algorithm and this mutation. 
                //
                // The original algorithm choose k element from a list and place them at the beginning of the list.
                // For more information of the original version. http://en.wikipedia.org/wiki/Reservoir_sampling
                //
                // This MUTATION place sepecified k elements to a randomly position.
                // That is, place <param>mines</param> in the <param>grid</param> randomly with equal posibility.
                // This method gives a better avg performance. Espicailly when there are a lot of mines.
                // Anyway, this will not be the bottolneck of this game.
                ///////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < mines; i++)
                {
                    grid[i].HasMine = true;
                }

                for (int i = mines; i < total; i++)
                {
                    int index = random.Next(i + 1);

                    if (index < i)
                    {
                        var mine = grid[index].HasMine;
                        grid[index].HasMine = grid[i].HasMine;
                        grid[i].HasMine = mine;
                    }
                }
                #endregion
            }
            else
            {
                #region Method 2 - Place mines directly to empty cell.
                //////////////////////////////////////////////////////////////////////////
                // This method works much better than the Reservoir Sampling Algorithm
                // When mines is much smaller than the grid size.
                /////////////////////////////////////////////////////////////////////////
                int minesToGenerate = mines;
                while (minesToGenerate > 0)
                {
                    int index = random.Next(total);
                    // Found an empty cell
                    // When mines to fill, near the size of the grid.
                    // There will be many times re-try that cause performance issues.
                    if (!grid[index].HasMine)
                    {
                        grid[index].HasMine = true;
                        --minesToGenerate;
                    }
                }
                #endregion
            }
        }
    }
}
