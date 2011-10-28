namespace ClearMine.Framework.Interactivity
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using ClearMine.Common.Localization;

    public class BehaviorCollection : ObservableCollection<Behavior>
    {
        public DependencyObject AttatchedObject { get; internal set; }

        public void AttatchTo(DependencyObject attachedObject)
        {
            if (attachedObject == null)
            {
                throw new ArgumentNullException("attachedObject");
            }

            if (AttatchedObject != null && attachedObject != AttatchedObject)
            {
                Trace.TraceWarning(LocalizationHelper.FindText("TraceReplaceBehaviorCollectionTarget"));
            }

            if (attachedObject != AttatchedObject)
            {
                AttatchedObject = attachedObject;
                DetatchAll();
                foreach (var behavior in this.Where(b => b.AttatchedObject != attachedObject))
                {
                    behavior.AttatchTo(attachedObject);
                }
            }
        }

        public void DetatchAll()
        {
            foreach (var behavior in this.Where(b => b.AttatchedObject != null))
            {
                behavior.Detatch();
            }
        }

        protected override void InsertItem(int index, Behavior item)
        {
            if (AttatchedObject != null)
            {
                item.AttatchTo(AttatchedObject);
            }

            base.InsertItem(index, item);
        }

        protected override void ClearItems()
        {
            DetatchAll();

            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            if (this[index].AttatchedObject != null)
            {
                this[index].Detatch();
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Behavior item)
        {
            if (this[index].AttatchedObject != null)
            {
                this[index].Detatch();
            }

            if (AttatchedObject != null)
            {
                item.AttatchTo(AttatchedObject);
            }

            base.SetItem(index, item);
        }
    }
}
