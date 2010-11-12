namespace ClearMine.Common.Log
{
    using System;
    using System.Diagnostics;

    public class TraceEventArgs : EventArgs
    {
        public TraceEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }

    public class RedirectorTraceListener : TraceListener
    {
        private static RedirectorTraceListener instance;

        public RedirectorTraceListener()
        {
            // Don't use any Trace of Debug in this constructor.
            // Otherwise may stack overflow exception.
            instance = this;
        }

        public static RedirectorTraceListener Current
        {
            get { return instance; }
        }

        public string Log { get; protected set; }

        public void Clear()
        {
            Log = String.Empty;
        }

        public override void Write(string message)
        {
            Log = String.Concat(Log, message);
            var temp = ShowMessage;
            if (temp != null)
            {
                temp(this, new TraceEventArgs(message));
            }
        }

        public override void WriteLine(string message)
        {
            var finalMessage = String.Concat(message, Environment.NewLine);
            Log = String.Concat(Log, finalMessage);
            var temp = ShowMessage;
            if (temp != null)
            {
                temp(this, new TraceEventArgs(finalMessage));
            }
        }

        public event EventHandler<TraceEventArgs> ShowMessage;
    }
}
