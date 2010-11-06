namespace ClearMine.Common.ComponentModel
{
    using System;

    internal interface ICachable<T> : IUpdatable<T>
        where T : class
    {
        CachingState CachingState { get; set; }

        event EventHandler CacheStateChanged;
    }
}
