namespace ClearMine.VM.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Controls;
    using ClearMine.Framework.Interactivity;
    using ClearMine.GameDefinition;

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
            AttatchedObject.MouseLeave += new MouseEventHandler(OnMineGroudMouseLeave);
            AttatchedObject.MouseEnter += new MouseEventHandler(OnMineGroudMouseEnter);
            Window.GetWindow(AttatchedObject).Closing += new CancelEventHandler(OnMainWindowClosing);
            Window.GetWindow(AttatchedObject).StateChanged += new EventHandler(OnMainWindowStateChanged);

            EventManager.RegisterClassHandler(typeof(MineCellControl), UIElement.MouseLeaveEvent, new MouseEventHandler(OnCellMouseLeave));
            EventManager.RegisterClassHandler(typeof(MineCellControl), UIElement.MouseEnterEvent, new MouseEventHandler(OnCellMouseEnter));
        }

        protected override void OnDetatching()
        {
            AttatchedObject.Loaded -= new RoutedEventHandler(OnAttatchedObjectLoaded);
            AttatchedObject.MouseUp -= new MouseButtonEventHandler(OnMineGroudMouseUp);
            AttatchedObject.MouseDown -= new MouseButtonEventHandler(OnMineGroudMouseDown);
            AttatchedObject.MouseLeave -= new MouseEventHandler(OnMineGroudMouseLeave);
            AttatchedObject.MouseEnter -= new MouseEventHandler(OnMineGroudMouseEnter);
            Window.GetWindow(AttatchedObject).Closing -= new CancelEventHandler(OnMainWindowClosing);
            Window.GetWindow(AttatchedObject).StateChanged -= new EventHandler(OnMainWindowStateChanged);
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

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                cell.PressState = PressState.Pressed;
            }

            vm.IsMousePressed = true;
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

            foreach (var c in vm.Cells)
            {
                c.PressState = PressState.Released;
            }

            if (Mouse.LeftButton == MouseButtonState.Released &&
                Mouse.RightButton == MouseButtonState.Released)
            {
                vm.IsMousePressed = false;
            }
        }

        private void OnMineGroudMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                vm.IsMousePressed = true;
            }
        }

        private void OnMineGroudMouseLeave(object sender, MouseEventArgs e)
        {
            vm.IsMousePressed = false;
        }

        private static void OnCellMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var cell = e.ExtractDataContext<MineCell>();
                if (cell != null)
                {
                    if (Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        cell.PressState = PressState.DoublePressed;
                        var vm = e.ExtractDataContext<ClearMineViewModel>();
                        foreach (var c in vm.Cells)
                        {
                            if (c.Near(cell) && (c.State == CellState.Normal || c.State == CellState.Question))
                            {
                                c.PressState = PressState.Pressed;
                            }
                        }
                    }
                    else
                    {
                        cell.PressState = PressState.Pressed;
                    }
                }
            }
        }

        private static void OnCellMouseLeave(object sender, MouseEventArgs e)
        {
            var vm = e.ExtractDataContext<ClearMineViewModel>();
            if (vm != null)
            {
                foreach (var c in vm.Cells)
                {
                    c.PressState = PressState.Released;
                }
            }
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

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            vm.RequestToClose(e);
        }
    }
}
