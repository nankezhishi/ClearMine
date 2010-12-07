namespace ClearMine.VM.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using ClearMine.Common.Logic;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Interactivity;

    public class MainInteractionBehavior : UIElementBehavior<FrameworkElement>
    {
        private bool expanding = false;
        private ClearMineViewModel vm;
        private DateTime? lastStateChangedTime;

        protected override void OnAttatched()
        {
            AutoDetatch = true;
            vm = AttatchedObject.DataContext as ClearMineViewModel;
            AttatchedObject.Loaded += new RoutedEventHandler(OnAttatchedObjectLoaded);
            AttatchedObject.MouseUp += new MouseButtonEventHandler(OnMineGroudMouseUp);
            AttatchedObject.MouseDown += new MouseButtonEventHandler(OnMineGroudMouseDown);
            Window.GetWindow(AttatchedObject).Closing += new CancelEventHandler(OnMainWindowClosing);
            Window.GetWindow(AttatchedObject).StateChanged += new EventHandler(OnMainWindowStateChanged);
        }

        protected override void OnDetatching()
        {
            AttatchedObject.Loaded -= new RoutedEventHandler(OnAttatchedObjectLoaded);
            AttatchedObject.MouseUp -= new MouseButtonEventHandler(OnMineGroudMouseUp);
            AttatchedObject.MouseDown -= new MouseButtonEventHandler(OnMineGroudMouseDown);
            Window.GetWindow(AttatchedObject).Closing -= new CancelEventHandler(OnMainWindowClosing);
            Window.GetWindow(AttatchedObject).StateChanged -= new EventHandler(OnMainWindowStateChanged);
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            vm.RequestToClose(e);
        }

        private void OnMineGroudMouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = e.ExtractDataContext<MineCell>();
            if (cell == null)
            {
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed &&
                Mouse.RightButton == MouseButtonState.Pressed)
            {
                vm.TryExpand(cell);
                expanding = true;
            }
            vm.IsMousePressed = true;
            vm.TriggerPropertyChanged("NewGameIcon");
        }

        private void OnMineGroudMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Maximim the window trigger a mouse up within the playground.
            // We need to block it here.
            if (lastStateChangedTime.HasValue && (DateTime.Now - lastStateChangedTime.Value).TotalMilliseconds < Math.Min(WindowsApi.GetDoubleClickInterval(), 300))
            {
                return;
            }

            var cell = e.ExtractDataContext<MineCell>();
            if (cell == null)
            {
                return;
            }

            if (e.ChangedButton == MouseButton.Left &&
                Mouse.RightButton == MouseButtonState.Released)
            {
                expanding = false;
                vm.DigAt(cell);
            }
            else if (e.ChangedButton == MouseButton.Right &&
                Mouse.LeftButton == MouseButtonState.Released)
            {
                if (expanding)
                {
                    expanding = false;
                }
                else
                {
                    vm.MarkAt(cell);
                }
            }
            vm.IsMousePressed = false;
            vm.TriggerPropertyChanged("NewGameIcon");
        }

        private void OnMainWindowStateChanged(object sender, EventArgs e)
        {
            if (Window.GetWindow(AttatchedObject).WindowState == WindowState.Maximized)
            {
                lastStateChangedTime = DateTime.Now;
            }
        }

        private void OnAttatchedObjectLoaded(object sender, RoutedEventArgs e)
        {
            vm.StartNewGame();
            vm.RefreshUI();
        }
    }
}
