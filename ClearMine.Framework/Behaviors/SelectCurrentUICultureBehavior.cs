namespace ClearMine.Framework.Behaviors
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Framework.Interactivity;

    /// <summary>
    /// 
    /// </summary>
    public class SelectCurrentUICultureBehavior : Behavior<MenuItem>
    {
        protected override void OnAttatched()
        {
            AttatchedObject.Loaded += new RoutedEventHandler(OnMenuLoaded);
        }

        private void OnMenuLoaded(object sender, RoutedEventArgs e)
        {
            AttatchedObject.Loaded -= new RoutedEventHandler(OnMenuLoaded);
            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            foreach (MenuItem item in AttatchedObject.Items)
            {
                if (currentCultureName.Equals(Convert.ToString(item.CommandParameter)))
                {
                    item.IsChecked = true;
                    break;
                }
            }
        }

        protected override void OnDetatching()
        {
            AttatchedObject.Loaded -= new RoutedEventHandler(OnMenuLoaded);
        }
    }
}
