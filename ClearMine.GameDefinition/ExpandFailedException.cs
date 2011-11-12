namespace ClearMine.GameDefinition
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ExpandFailedException : Exception
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

        protected ExpandFailedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
