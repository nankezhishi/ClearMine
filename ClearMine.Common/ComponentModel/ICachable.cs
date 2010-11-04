namespace ClearMine.Common.ComponentModel
{
    using System;

    internal interface ICachable<T>
        where T : class
    {
        void Update(T newValue);

        CachingState CachingState { get; set; }

        event EventHandler CacheStateChanged;
    }
}
