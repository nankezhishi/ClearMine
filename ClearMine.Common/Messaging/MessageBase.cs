namespace ClearMine.Common.Messaging
{
    public abstract class MessageBase
    {
        public bool Handled { get; set; }

        public object Source { get; set; }

        public object HandlingResult { get; set; }
    }
}
