namespace ClearMine.Framework.Messages
{
    using System;
    using ClearMine.Common.Messaging;

    public class ExceptionMessage : MessageBase
    {
        public Exception Exception { get; protected set; }

        public ExceptionMessage(Exception exception)
        {
            Exception = exception;
        }
    }
}
