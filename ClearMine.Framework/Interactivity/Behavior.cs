namespace ClearMine.Framework.Interactivity
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using ClearMine.Localization;

    public abstract class Behavior : DependencyObject
    {
        protected abstract void OnAttatched();
        protected abstract void OnDetatching();

        public DependencyObject AttatchedObject { get; protected set; }

        internal void AttatchTo(DependencyObject attatchedObject)
        {
            if (AttatchedObject != null)
            {
                Detatch();
                Trace.TraceWarning(LocalizationHelper.FindText("TraceReplaceBehaviorTarget", this.ToString()));
            }

            AttatchedObject = attatchedObject;
            OnAttatched();
        }

        internal void Detatch()
        {
            OnDetatching();
        }
    }

    public abstract class Behavior<T> : Behavior
        where T : DependencyObject
    {
        public new T AttatchedObject
        {
            get { return base.AttatchedObject as T; }
        }
    }

    public abstract class UIElementBehavior<T> : Behavior<T>
        where T : FrameworkElement
    {
        private bool autoDetatch = false;

        public bool AutoDetatch
        {
            get { return autoDetatch; }
            set
            {
                if (AttatchedObject == null)
                {
                    throw new InvalidOperationException(LocalizationHelper.FindText("DetatchingUnattachedBehavior"));
                }

                if (autoDetatch != value)
                {
                    if (value)
                    {
                        AttatchedObject.Unloaded += new RoutedEventHandler(OnAttatchedObjectUnloaded);
                    }
                    else
                    {
                        AttatchedObject.Unloaded -= new RoutedEventHandler(OnAttatchedObjectUnloaded);
                    }
                }
            }
        }

        private void OnAttatchedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            Debug.Assert(AutoDetatch);
            AutoDetatch = false;
            Detatch();
        }
    }
}
