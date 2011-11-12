﻿namespace ClearMine.Framework.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    public static class Interaction
    {
        public static T FindBehavior<T>(this DependencyObject element, Action<T> action = null)
            where T : Behavior
        {
            var current = element;
            while (current != null)
            {
                var behaviors = GetBehaviors(current);
                foreach (var behavior in behaviors)
                {
                    var typedBehavior = behavior as T;
                    if (typedBehavior != null)
                    {
                        if (action != null)
                        {
                            action(typedBehavior);
                        }

                        return typedBehavior;
                    }
                }

                current = VisualTreeHelper.GetParent(current) ?? ((dynamic)current).Parent ?? ((dynamic)current).TemplatedParent;
            }

            return null;
        }

        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            if (obj == null)
                return null;

            var result = obj.GetValue(BehaviorsProperty) as BehaviorCollection;
            if (result == null)
            {
                result = new BehaviorCollection();
                obj.SetValue(BehaviorsProperty, result);
            }

            return result;
        }

        public static void SetBehaviors(DependencyObject obj, BehaviorCollection value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.SetValue(BehaviorsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Behaviors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof(BehaviorCollection), typeof(Interaction), new UIPropertyMetadata(null, new PropertyChangedCallback(OnBehaviorsPropertyChanged)));

        private static void OnBehaviorsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                (e.OldValue as BehaviorCollection).DetachAll();
            }

            if (e.NewValue != null)
            {
                (e.NewValue as BehaviorCollection).AttachTo(sender);
            }
        }
    }
}
