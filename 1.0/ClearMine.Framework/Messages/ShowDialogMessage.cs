namespace ClearMine.Framework.Messages
{
    using System.Windows;

    using ClearMine.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public class ShowDialogMessage : MessageBase
    {
        public ShowDialogMessage()
        {
            ModuleDialog = true;
        }

        public ShowDialogMessage(PopupDialog type)
            : this()
        {
            DialogType = type;
        }

        public ShowDialogMessage(PopupDialog type, object data)
            : this(type)
        {
            Data = data;
        }

        public ShowDialogMessage(DependencyObject source, PopupDialog type)
            : this(type)
        {
            Source = source;
        }

        public ShowDialogMessage(object source, PopupDialog type, object data)
            : this(type, data)
        {
            Source = source;
        }

        public ShowDialogMessage(object source, PopupDialog type, bool moduleDialog)
            : this(type, (object)null)
        {
            Source = source;
            ModuleDialog = moduleDialog;
        }

        public PopupDialog DialogType { get; set; }

        public object Data { get; set; }

        public bool ModuleDialog { get; set; }
    }
}
