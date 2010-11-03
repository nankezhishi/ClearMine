namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CachedObservableCollection<T> : ObservableCollection<T>
        where T : class, IUpdatable<T>
    {
        private int countInUse = 0;

        protected override void ClearItems()
        {
            // Don't worry about the redundent items.
            // UI Layer should take the resposiblity of not showing them.
            // But we still need to clear mines on any initialize.

            countInUse = 0;
        }

        protected override void RemoveItem(int index)
        {
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
                    MoveItem(countInUse, index);
                }

                this[index].Update(item);
            }
            else
            {
                base.InsertItem(index, item);
            }
            ++countInUse;
        }

        protected override void SetItem(int index, T item)
        {
            if (index < countInUse)
            {
                this[index].Update(item);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }
    }
}
