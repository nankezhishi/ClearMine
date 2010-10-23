namespace ClearMine.UI.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using ClearMine.Framework;
    using ClearMine.Logic;

    internal class ClearMineViewModel : ViewModelBase
    {
        private IClearMine game = new ClearMineGame();

        public ClearMineViewModel()
        {
            game.StateChanged += new EventHandler(OnGameStateChanged);
        }

        public int Columns
        {
            get { return (int)game.Size.Width; }
        }

        public int Rows
        {
            get { return (int)game.Size.Height; }
        }

        public IEnumerable<MineCell> Cells
        {
            get { return game.Cells; }
        }

        public void Start(Size size, int mines)
        {
            game.Initialize(size, mines);
            OnPropertyChanged("Columns");
            OnPropertyChanged("Rows");
        }

        public void MarkAsMine(MineCell cell)
        {
            game.TryMarkAt(cell, CellState.MarkAsMine);
        }

        public void DigAt(MineCell cell)
        {
            game.TryDigAt(cell);
        }

        private void OnGameStateChanged(object sender, EventArgs e)
        {
            if (game.GameState == GameState.Failed)
            {
                var lostWindow = new GameLostWindow();
                lostWindow.Owner = Application.Current.MainWindow;
                if ((bool)lostWindow.ShowDialog())
                {
                    game.StartNew();
                }
                else
                {
                    game.Restart();
                }
            }
            else if (game.GameState == GameState.Success)
            {
                var wonWindow = new GameWonWindow();
                wonWindow.Owner = Application.Current.MainWindow;
                if ((bool)wonWindow.ShowDialog())
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    game.StartNew();
                }
            }
        }
    }
}
