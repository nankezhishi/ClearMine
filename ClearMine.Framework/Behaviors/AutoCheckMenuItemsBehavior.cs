namespace ClearMine.Framework.Behaviors
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
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

        /// <summary>
        /// 
        /// </summary>
        public string CurrentValueStoragePropertyName { get; set; }

        protected override void OnAttached()
        {
            AttachedObject.Click += new RoutedEventHandler(OnMenuItemClick);
            AttachedObject.Initialized += new EventHandler(OnMenuItemInitialized);
        }

        protected override void OnDetaching()
        {
            AttachedObject.Click -= new RoutedEventHandler(OnMenuItemClick);
            AttachedObject.Initialized -= new EventHandler(OnMenuItemInitialized);
        }

        private void OnMenuItemInitialized(object sender, EventArgs e)
        {
            SelectItemFromViewModel();
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
                    Trace.TraceWarning(ResourceHelper.FindText("AutoCheckIncompatibleMessage"));
                }

                if (child.IsChecked)
                {
                    previousCheckedItem = child;
                    child.IsChecked = false;
                    break;
                }
            }

            menuToCheck.IsChecked = true;
            try
            {
                Settings.Default[CurrentValueStoragePropertyName] = menuToCheck.CommandParameter;
            }
            catch
            {
                Trace.TraceWarning(ResourceHelper.FindText("AutoCheckCannotStoreSelection"));
            }
        }

        private void SelectItemFromViewModel()
        {
            var currentValue = Settings.Default[CurrentValueStoragePropertyName] as string;
            foreach (var item in AttachedObject.Items)
            {
                var menuItem = item as MenuItem;
                if (menuItem != null)
                {
                    menuItem.IsChecked = String.CompareOrdinal(menuItem.CommandParameter as string, currentValue) == 0;
                }
            }
        }
    }
}
