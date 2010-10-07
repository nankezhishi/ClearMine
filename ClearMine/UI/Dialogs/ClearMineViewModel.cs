namespace ClearMine.UI.Dialogs
{
    using System.Collections.Generic;

    using ClearMine.Framework;
    using ClearMine.Logic;

    internal class ClearMineViewModel : ViewModelBase
    {
        private ClearMineGame game = new ClearMineGame();

        public ClearMineViewModel()
        {
        }

        public void Start(int width, int height, int mines)
        {
            game.Initialize(width, height, mines);
        }

        public void MarkAsMine(MineCell cell)
        {
            game.TryMarkAt(cell, CellState.MarkAsMine);
        }

        public void DigAt(MineCell cell)
        {
            if (!game.TryDigAt(cell))
            {
                game.StartNew();
            }
        }

        public IEnumerable<MineCell> Cells
        {
            get { return game.Cells; }
        }
    }
}
