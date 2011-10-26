using System;
using ClearMine.Common.Messaging;

namespace ClearMine.Framework.Dialogs
{
    public class ShowDialogMessage : MessageBase
    {
        public Type DialogType { get; set; }

        public object Data { get; set; }
    }
}
