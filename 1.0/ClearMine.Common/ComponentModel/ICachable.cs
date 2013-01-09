namespace ClearMine.Common.ComponentModel
{
    using System;

    public interface ICachable<T> : IUpdatable<T>
        where T : class
    {
        CachingState CachingState { get; set; }

        event EventHandler CacheStateChanged;
    }
}
