namespace ClearMine.Common.Messaging
{
    public abstract class MessageBase
    {
        public virtual bool Handled { get; set; }

        public virtual object Source { get; set; }

        public virtual object HandlingResult { get; set; }
    }
}
