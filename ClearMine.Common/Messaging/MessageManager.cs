namespace ClearMine.Common.Messaging
{
    using System;
    using System.Collections.Generic;

    public class MessageManager
    {
        private static IList<object> aggregators = new List<object>();

        public static MessageAggregator<T> GetMessageAggregator<T>()
            where T : MessageBase
        {
            foreach (var aggregator in aggregators)
            {
                if (aggregator is MessageAggregator<T>)
                {
                    return aggregator as MessageAggregator<T>;
                }
            }

            var newOne = Activator.CreateInstance<MessageAggregator<T>>();
            aggregators.Add(newOne);

            return newOne;
        }
    }
}
