namespace ClearMine.Framework.Behaviors
{
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Framework.Interactivity;

    public class AutoCheckMenuItemsBehavior : Behavior<MenuItem>
    {
        private MenuItem previousCheckedItem;

        /// <summary>
        /// 
        /// </summary>
        public void UndoMenuItemCheck()
        {
            if (previousCheckedItem != null)
            {
                ResetMenuState(previousCheckedItem);
                previousCheckedItem = null;
            }
        }

        protected override void OnAttached()
        {
            AttachedObject.Click += new RoutedEventHandler(OnMenuItemClick);
        }

        protected override void OnDetaching()
        {
            AttachedObject.Click -= new RoutedEventHandler(OnMenuItemClick);
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            #region Guards
            // Ignore click on itself.
            if (e.OriginalSource == AttachedObject)
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
            if (parent != AttachedObject)
            {
                return;
            }
            #endregion

            ResetMenuState(menuItem);
        }

        private void ResetMenuState(MenuItem menuToCheck)
        {
            var parent = menuToCheck.Parent as ItemsControl;

            foreach (MenuItem child in parent.Items.Cast<UIElement>().Where(m => m is MenuItem))
            {
                if (child.IsCheckable)
                {
                    Trace.TraceWarning("AutoCheckMenuItemsBehavior may not be able to work with a checkable menu item correctly.");
                }

                if (child.IsChecked)
                {
                    previousCheckedItem = child;
                    child.IsChecked = false;
                    break;
                }
            }

            menuToCheck.IsChecked = true;
        }
    }
}
