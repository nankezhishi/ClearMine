namespace ClearMine.Framework
{
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Framework.Dialogs;

    public class FrameworkModule : IModule
    {
        public void InitializeModule()
        {
            MessageManager.GetMessageAggregator<ExceptionMessage>().Subscribe(new ExceptionMessageProcessor().HandleMessage);
        }
    }
}
