namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using System.Windows.Input;

    using ClearMine.Common.Utilities;
    using ClearMine.Logic;
    using ClearMine.Properties;

    /// <summary>
    /// Interaction logic for ClearMineWindow.xaml
    /// </summary>
    internal partial class ClearMineWindow : Window
    {
        private bool expanding = false;
        private ClearMineViewModel vm = new ClearMineViewModel();

        public ClearMineWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            vm.Start();
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
    }
}
