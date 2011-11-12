namespace ClearMine.Framework.Behaviors
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Framework.Interactivity;

    /// <summary>
    /// 
    /// </summary>
    public class SelectCurrentUICultureBehavior : Behavior<MenuItem>
    {
        protected override void OnAttached()
        {
            AttachedObject.Loaded += new RoutedEventHandler(OnMenuLoaded);
        }

        private void OnMenuLoaded(object sender, RoutedEventArgs e)
        {
            AttachedObject.Loaded -= new RoutedEventHandler(OnMenuLoaded);
            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            foreach (MenuItem item in AttachedObject.Items)
            {
                if (currentCultureName.Equals(Convert.ToString(item.CommandParameter, CultureInfo.InvariantCulture)))
                {
                    item.IsChecked = true;
                    break;
                }
            }
        }

        protected override void OnDetaching()
        {
            AttachedObject.Loaded -= new RoutedEventHandler(OnMenuLoaded);
        }
    }
}
