namespace ClearMine.Framework.Behaviors
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ClearMine.Framework.Interactivity;

    public class AutoCheckMenuItemsBehavior : Behavior<MenuItem>
    {
        protected override void OnAttatched()
        {
            AttatchedObject.Click += new RoutedEventHandler(OnMenuItemClick);
        }

        protected override void OnDetatching()
        {
            AttatchedObject.Click -= new RoutedEventHandler(OnMenuItemClick);
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            #region Guards
            // Ignore click on itself.
            if (e.OriginalSource == AttatchedObject)
            {
                return;
            }

            // Ignore already checked.
            var menuItem = e.OriginalSource as MenuItem;
            if (menuItem.IsChecked)
            {
                return;
            }

            // Ingore sub menu of sub menu.
            var parent = (menuItem.Parent as MenuItem);
            if (parent != AttatchedObject)
            {
                return;
            }
            #endregion

            ResetMenuState(menuItem);
        }

        private void ResetMenuState(MenuItem menuToCheck)
        {
            foreach (MenuItem child in AttatchedObject.Items.Cast<UIElement>().Where(m => m is MenuItem))
            {
                if (child.IsCheckable)
                {
                    Trace.TraceWarning("AutoCheckMenuItemsBehavior may not be able to work with a checkable menu item correctly.");
                }

                child.IsChecked = false;
            }

            menuToCheck.IsChecked = true;
        }
    }
}
