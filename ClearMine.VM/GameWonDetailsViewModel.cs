﻿namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;

    internal class GameWonDetailsViewModel : ViewModelBase
    {
        private HeroHistory history;

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            throw new NotImplementedException();
        }

        public GameWonDetailsViewModel()
        {
            Debug.Assert(Settings.Default.Difficulty != Difficulty.Custom);
            history = Settings.Default.HeroList.GetByLevel(Settings.Default.Difficulty);
        }

        public double BestTime
        {
            get { return (from item in history.Items select item.Score).Min(); }
        }

        public int GamePlayed
        {
            get { return history.GamePlayed; }
        }

        public int GameWon
        {
            get { return history.GameWon; }
        }

        public double WonRate
        {
            get { return history.GameWonPercentage; }
        }
    }
}