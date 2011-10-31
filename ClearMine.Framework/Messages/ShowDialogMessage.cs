namespace ClearMine.Framework.Messages
{
    using System;
    using ClearMine.Common.Messaging;

    public class ShowDialogMessage : MessageBase
    {
        public ShowDialogMessage()
        {
            ModuleDialog = true;
        }

        public Type DialogType { get; set; }

        public object Data { get; set; }

        public bool ModuleDialog { get; set; }
    }
}
