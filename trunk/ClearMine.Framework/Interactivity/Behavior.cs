namespace ClearMine.Framework.Interactivity
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using ClearMine.Common.Utilities;

    public abstract class Behavior : DependencyObject
    {
        protected abstract void OnAttached();
        protected abstract void OnDetaching();

        public DependencyObject AttachedObject { get; protected set; }

        internal void AttatchTo(DependencyObject attachedObject)
        {
            if (AttachedObject != null)
            {
                Detatch();
                Trace.TraceWarning(ResourceHelper.FindText("TraceReplaceBehaviorTarget", this));
            }

            AttachedObject = attachedObject;
            OnAttached();
        }

        internal void Detatch()
        {
            OnDetaching();
        }
    }

    public abstract class Behavior<T> : Behavior
        where T : DependencyObject
    {
        public new T AttachedObject
        {
            get { return base.AttachedObject as T; }
        }
    }

    public abstract class UIElementBehavior<T> : Behavior<T>
        where T : FrameworkElement
    {
        private bool autoDetach = false;

        public bool AutoDetach
        {
            get { return autoDetach; }
            set
            {
                if (AttachedObject == null)
                {
                    throw new InvalidOperationException(ResourceHelper.FindText("DetatchingUnattachedBehavior"));
                }

                if (autoDetach != value)
                {
                    if (value)
                    {
                        AttachedObject.Unloaded += OnAttachedObjectUnloaded;
                    }
                    else
                    {
                        AttachedObject.Unloaded -= OnAttachedObjectUnloaded;
                    }
                }
            }
        }

        private void OnAttachedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            AutoDetach = false;
            Detatch();
        }
    }
}
