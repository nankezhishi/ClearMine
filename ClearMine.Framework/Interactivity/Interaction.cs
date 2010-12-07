namespace ClearMine.Framework.Interactivity
{
    using System.Windows;
    using System.Collections;

    public class Interaction
    {
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
