namespace ClearMine.Common
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GenericEventArgs()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public GenericEventArgs(T data)
        {
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}
