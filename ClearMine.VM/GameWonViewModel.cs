namespace ClearMine.VM
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using ClearMine.Common;
    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.VM.Commands;

    public class GameWonViewModel : ViewModelBase
    {
        public GameWonViewModel(double usedTime, DateTime time)
        {
            Time = time;
            TimeUsed = usedTime;
            Message = ResourceHelper.FindText("GameWonMessage");

            if (Settings.Default.Difficulty != Difficulty.Custom)
            {
                Details = new GameWonDetailsViewModel();
                if (usedTime == Details.BestTime)
                {
                    // Tell created a record.
                    Message += String.Empty;
                }
            }
        }

        public override IEnumerable<CommandBinding> CommandBindings
        {
            get { return GameCommandBindings.GameWonCommandBindings; }
        }

        public string Message { get; private set; }

        public double TimeUsed { get; private set; }

        public DateTime Time { get; private set; }

        public GameWonDetailsViewModel Details { get; private set; }
    }
}
