﻿namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Properties;
    using ClearMine.Localization;
    using ClearMine.VM.Commands;

    internal class GameWonViewModel : ViewModelBase
    {
        public GameWonViewModel(double usedTime, DateTime time)
        {
            Time = time;
            TimeUsed = usedTime;
            Message = LocalizationHelper.FindText("GameWonMessage");

            if (Settings.Default.Difficulty != Difficulty.Custom)
            {
                Details = new GameWonDetailsViewModel();
                if (usedTime == Details.BestTime)
                {
                    // Tell created a record.
                    Message += "";
                }
            }
        }

        public override IEnumerable<CommandBinding> GetCommandBindings()
        {
            yield return GameCommandBindings.StatisticsBinding;
        }

        public string Message { get; private set; }

        public double TimeUsed { get; private set; }

        public DateTime Time { get; private set; }

        public GameWonDetailsViewModel Details { get; private set; }
    }
}