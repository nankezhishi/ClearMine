namespace ClearMine.Common.Messaging
{
    using System;
    using System.Collections.Generic;

    public static class MessageManager
    {
        private static IList<object> aggregators = new List<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public static void SubscribeMessage<T>(Action<T> handler)
            where T : MessageBase
        {
            GetMessageAggregator<T>().Subscribe(handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object SendMessage<T>(params object[] args)
            where T : MessageBase
        {
            return MessageManager
                .GetMessageAggregator<T>()
                .SendMessage(Activator.CreateInstance(typeof(T), args) as T)
                .HandlingResult;
        }

        internal static MessageAggregator<T> GetMessageAggregator<T>()
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
