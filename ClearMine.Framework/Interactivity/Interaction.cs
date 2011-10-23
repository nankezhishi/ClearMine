namespace ClearMine.Framework.Interactivity
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
                    if (behavior is T)
                    {
                        if (action != null)
                        {
                            action(behavior as T);
                        }

                        return behavior as T;
                    }
                }

                current = VisualTreeHelper.GetParent(current) ?? ((dynamic)current).Parent ?? ((dynamic)current).TemplatedParent;
            }

            return null;
        }

        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
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
            obj.SetValue(BehaviorsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Behaviors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof(BehaviorCollection), typeof(Interaction), new UIPropertyMetadata(null, new PropertyChangedCallback(OnBehaviorsPropertyChanged)));

        private static void OnBehaviorsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                (e.OldValue as BehaviorCollection).DetatchAll();
            }

            if (e.NewValue != null)
            {
                (e.NewValue as BehaviorCollection).AttatchTo(sender);
            }
        }
    }
}
