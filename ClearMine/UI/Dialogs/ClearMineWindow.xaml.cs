namespace ClearMine.UI.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.Utilities;
    using ClearMine.Logic;

    /// <summary>
    /// Interaction logic for ClearMineWindow.xaml
    /// </summary>
    internal partial class ClearMineWindow : Window
    {
        private bool expanding = false;
        private ClearMineViewModel vm = new ClearMineViewModel();
        private DateTime? lastStateChangedTime;

        public ClearMineWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            vm.RequestToClose(e);
            base.OnClosing(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            vm.StartNewGame();
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
        }

        private void OnMineGroudMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Maximim the window trigger a mouse up within the playground.
            // We need to block it here.
            if (lastStateChangedTime.HasValue && (DateTime.Now - lastStateChangedTime.Value).TotalMilliseconds < Math.Min(Win32.GetDoubleClickTime(), 300))
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
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                lastStateChangedTime = DateTime.Now;
            }
        }
    }
}
