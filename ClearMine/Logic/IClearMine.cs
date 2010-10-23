namespace ClearMine.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    interface IClearMine
    {
        void Initialize(Size size, int mines);

        void Restart();

        void StartNew();

        void TryMarkAt(MineCell cell, CellState newState);

        GameState TryDigAt(MineCell cell);

        bool TryExpandAt(MineCell cell);

        MineCell GetCell(int column, int row);

        int UsedTime { get; }

        int TotalMines { get; }

        int RemainedMines { get; }

        Size Size { get; }

        IEnumerable<MineCell> Cells { get; }

        GameState GameState { get; set; }

        event EventHandler StateChanged;
    }
}
