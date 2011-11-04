namespace ClearMine.Framework
{
    using ClearMine.Common.Messaging;
    using ClearMine.Common.Modularity;
    using ClearMine.Framework.Messages;

    public class FrameworkModule : IModule
    {
        public void InitializeModule()
        {
            MessageManager.SubscribeMessage<ExceptionMessage>(new ExceptionMessageProcessor().HandleMessage);
        }
    }
}
