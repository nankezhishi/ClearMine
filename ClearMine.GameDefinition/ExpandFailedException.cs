namespace ClearMine.GameDefinition
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ExpandFailedException : ApplicationException
    {
        public ExpandFailedException()
            : base()
        {
        }

        public ExpandFailedException(string message)
            : base(message)
        {
        }

        public ExpandFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ExpandFailedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
