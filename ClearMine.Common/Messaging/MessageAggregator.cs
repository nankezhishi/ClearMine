namespace ClearMine.Common.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal class MessageAggregator<T> where T : MessageBase
    {
        private IDictionary<ListeningPriority, List<Action<T>>> processors = new Dictionary<ListeningPriority, List<Action<T>>>();

        public void Subscribe(Action<T> processor, ListeningPriority priority = ListeningPriority.Normal)
        {
            if (!processors.ContainsKey(priority))
                processors.Add(priority, new List<Action<T>>());

            if (!processors.Values.Any(p => p.Contains(processor)))
            {
                processors[priority].Add(processor);
            }
        }

        public void Unsubscribe(Action<T> processor)
        {
            EnumerateAliveProcessors((r, g) =>
            {
                if (r == processor)
                {
                    g.Value.Remove(r);
                }
            });
        }

        public T SendMessage(T message)
        {
            if (message != null)
            {
                EnumerateAliveProcessors((r, g) => (r as Action<T>)(message));
            }

            return message;
        }

        // TODO: Maybe a post message method if any place need.

        private void EnumerateAliveProcessors(Action<Action<T>, KeyValuePair<ListeningPriority, List<Action<T>>>> action)
        {
            foreach (var groupProcessors in processors.OrderBy(p => p.Key))
            {
                // ToList to make a copy of processors in case of adding new processor
                foreach (var reference in groupProcessors.Value.ToList())
                {
                    try
                    {
                        action(reference, groupProcessors);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                    }
                }
            }
        }
    }
}
