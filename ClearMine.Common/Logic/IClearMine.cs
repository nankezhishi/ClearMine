﻿namespace ClearMine.Common.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using ClearMine.Common.ComponentModel;

    public interface IClearMine : IUpdatable<IClearMine>
    {
        void Initialize(Size size, int mines);

        void Restart();

        void StartNew();

        void PauseGame();

        void ResumeGame();

        bool CheckHash();

        MineCell GetCell(int column, int row);

        void TryMarkAt(MineCell cell, CellState newState);

        IEnumerable<MineCell> TryDigAt(MineCell cell);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>All the mines that has been shown </returns>
        IEnumerable<MineCell> TryExpandAt(MineCell cell);

        int UsedTime { get; }

        int TotalMines { get; }

        int RemainedMines { get; }

        Size Size { get; }

        IEnumerable<MineCell> Cells { get; }

        GameState GameState { get; }

        event EventHandler StateChanged;

        event EventHandler TimeChanged;

        event EventHandler<CellStateChangedEventArgs> CellStateChanged;
    }
}
