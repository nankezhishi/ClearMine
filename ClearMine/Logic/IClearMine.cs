namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;

    interface IClearMine
    {
        void Initialize(int width, int height, int mines);

        void StartNew();

        void TryMarkAt(MineCell cell, CellState newState);

        bool TryDigAt(MineCell cell);

        bool TryExpandAt(MineCell cell);

        MineCell GetCell(int column, int row);

        int UsedTime { get; }

        int TotalMines { get; }

        int RemainedMines { get; }

        IEnumerable<MineCell> Cells { get; }

        GameState GameState { get; set; }

        event EventHandler StateChanged;
    }
}
