namespace ClearMine.GameDefinition.Utilities
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public static class GameExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public static void UpdateStatistics(this IClearMine game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            var history = Settings.Default.HeroList.GetByLevel(Settings.Default.Difficulty);
            if (history != null)
            {
                if (game.GameState == GameState.Success)
                {
                    var target = VisualTreeHelper.GetChild(Application.Current.MainWindow, 0) as FrameworkElement;
                    var filePath = Infrastructure.Container.GetExportedValue<IVisualShoot>().SaveSnapshoot(target);

                    history.IncreaseWon(game.UsedTime / 1000.0, DateTime.Now, filePath);
                }
                else if (game.GameState == GameState.Failed)
                {
                    history.IncreaseLost();
                }
                else
                {
                    history.IncreaseUndone();
                }
            }

            Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static bool AutoMarkAt(this IClearMine game, MineCell cell)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            if (cell == null)
                throw new ArgumentNullException("cell");

            if (game.GameState == GameState.Initialized || game.GameState == GameState.Started)
            {
                if (cell.State == CellState.Normal)
                {
                    game.TryMarkAt(cell, CellState.MarkAsMine);
                }
                else if (cell.State == CellState.MarkAsMine)
                {
                    if (Settings.Default.ShowQuestionMark)
                    {
                        game.TryMarkAt(cell, CellState.Question);
                    }
                    else
                    {
                        game.TryMarkAt(cell, CellState.Normal);
                    }
                }
                else if (cell.State == CellState.Question)
                {
                    game.TryMarkAt(cell, CellState.Normal);
                }
                else
                {
                    // Do nothing.
                }

                return true;
            }

            return false;
        }
    }
}
