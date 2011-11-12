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
                Trace.TraceWarning(LocalizationHelper.FindText("TraceReplaceBehaviorTarget", this));
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
                    throw new InvalidOperationException(LocalizationHelper.FindText("DetatchingUnattachedBehavior"));
                }

                if (autoDetach != value)
                {
                    if (value)
                    {
                        AttachedObject.Unloaded += new RoutedEventHandler(OnAttachedObjectUnloaded);
                    }
                    else
                    {
                        AttachedObject.Unloaded -= new RoutedEventHandler(OnAttachedObjectUnloaded);
                    }
                }
            }
        }

        private void OnAttachedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(AutoDetach);
            AutoDetach = false;
            Detatch();
        }
    }
}
