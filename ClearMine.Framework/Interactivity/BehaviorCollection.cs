namespace ClearMine.Framework.Interactivity
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using ClearMine.Common.Utilities;

    public class BehaviorCollection : ObservableCollection<Behavior>
    {
        public DependencyObject AttachedObject { get; internal set; }

        public void AttachTo(DependencyObject attachedObject)
        {
            if (attachedObject == null)
            {
                throw new ArgumentNullException("attachedObject");
            }

            if (AttachedObject != null && attachedObject != AttachedObject)
            {
                Trace.TraceWarning(ResourceHelper.FindText("TraceReplaceBehaviorCollectionTarget"));
            }

            if (attachedObject != AttachedObject)
            {
                AttachedObject = attachedObject;
                DetachAll();
                foreach (var behavior in this.Where(b => b.AttachedObject != attachedObject))
                {
                    behavior.AttatchTo(attachedObject);
                }
            }
        }

        public void DetachAll()
        {
            foreach (var behavior in this.Where(b => b.AttachedObject != null))
            {
                behavior.Detatch();
            }
        }

        protected override void InsertItem(int index, Behavior item)
        {
            if (AttachedObject != null && item != null)
            {
                item.AttatchTo(AttachedObject);
            }

            base.InsertItem(index, item);
        }

        protected override void ClearItems()
        {
            DetachAll();

            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            if (this[index].AttachedObject != null)
            {
                this[index].Detatch();
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Behavior item)
        {
            if (this[index].AttachedObject != null)
            {
                this[index].Detatch();
            }

            if (AttachedObject != null && item != null)
            {
                item.AttatchTo(AttachedObject);
            }

            base.SetItem(index, item);
        }
    }
}
