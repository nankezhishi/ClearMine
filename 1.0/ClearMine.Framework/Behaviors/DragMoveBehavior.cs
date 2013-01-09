namespace ClearMine.Framework.Behaviors
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Framework.Interactivity;

    /// <summary>
    /// 
    /// </summary>
    public class DragMoveBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            AttachedObject.MouseDown += OnUIElementMouseDown;
        }

        protected override void OnDetaching()
        {
            AttachedObject.MouseDown -= OnUIElementMouseDown;
        }

        private static void OnUIElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var window = Window.GetWindow(sender as DependencyObject);
                if (window != null)
                    window.DragMove();
                else
                    Trace.TraceError("Cannot find the main window to drag.");
            }
        }
    }
}
