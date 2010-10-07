namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;

    interface IClearMine
    {
        void Initialize(int width, int height, int mines);

        void StartNew();

        void TryMarkAt(int column, int row, CellState newState);

        bool TryDigAt(int column, int row);

        bool TryExpandAt(int column, int row);

        MineCell GetCell(int column, int row);

        int UsedTime { get; }

        int TotalMines { get; }

        int RemainedMines { get; }

        IEnumerable<MineCell> Cells { get; }

        GameState GameState { get; set; }

        event EventHandler StateChanged;
    }
}
