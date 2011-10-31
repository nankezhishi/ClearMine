namespace ClearMine.Dialogs
{
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Framework.Messages;

    /// <summary>
    /// 
    /// </summary>
    public class DialogModule : IModule
    {
        public void InitializeModule()
        {
            MessageManager.GetMessageAggregator<ShowDialogMessage>().Subscribe(new DialogMessageProcessor().HandleMessage);
        }
    }
}
