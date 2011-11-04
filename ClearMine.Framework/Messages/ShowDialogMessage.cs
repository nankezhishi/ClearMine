namespace ClearMine.Framework.Messages
{
    using System;
    using System.Windows;
    using ClearMine.Common.Messaging;

    public class ShowDialogMessage : MessageBase
    {
        public ShowDialogMessage()
        {
            ModuleDialog = true;
        }

        public ShowDialogMessage(Type type)
            : this()
        {
            DialogType = type;
        }

        public ShowDialogMessage(Type type, object data)
            : this(type)
        {
            Data = data;
        }

        public ShowDialogMessage(DependencyObject source, Type type)
            : this(type)
        {
            Source = source;
        }

        public ShowDialogMessage(object source, Type type, object data)
            : this(type, data)
        {
            Source = source;
        }

        public ShowDialogMessage(object source, Type type, bool moduleDialog)
            : this(type, (object)null)
        {
            Source = source;
            ModuleDialog = moduleDialog;
        }

        public Type DialogType { get; set; }

        public object Data { get; set; }

        public bool ModuleDialog { get; set; }
    }
}
