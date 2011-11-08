namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public class CachedObservableCollection<T> : ObservableCollection<T>
        where T : class, ICachable<T>
    {
        private int countInUse = 0;

        protected override void ClearItems()
        {
            // Don't worry about the redundent items.
            // UI Layer should take the resposiblity of not showing them.
            // But we still need to clear mines on any initialize.

            countInUse = 0;
            foreach (T item in this)
            {
                item.CachingState = CachingState.InCache;
            }
        }

        protected override void RemoveItem(int index)
        {
            this[index].CachingState = CachingState.InCache;

            // Move the item to be remove to the end;
            MoveItem(index, Count - 1);

            --countInUse;
        }

        protected override void InsertItem(int index, T item)
        {
            if (index > countInUse)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (Count > countInUse)
            {
                if (index < countInUse)
                {
                    Trace.TraceWarning(LocalizationHelper.FindText("PerformanceIssueMoving"));

                    MoveItem(countInUse, index);
                }

                item.CachingState = CachingState.Disposed;
                this[index].CachingState = CachingState.InUse;
                this[index].Update(item);
            }
            else
            {
                item.CachingState = CachingState.InUse;
                base.InsertItem(index, item);
            }

            ++countInUse;
        }

        protected override void SetItem(int index, T item)
        {
            if (index < countInUse)
            {
                item.CachingState = CachingState.Disposed;
                this[index].Update(item);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }
    }
}
