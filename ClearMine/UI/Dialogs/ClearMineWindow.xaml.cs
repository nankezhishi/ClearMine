namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using System.Windows.Input;
    using ClearMine.Utilities;
    using ClearMine.Logic;

    /// <summary>
    /// Interaction logic for ClearMineWindow.xaml
    /// </summary>
    public partial class ClearMineWindow : Window
    {
        private ClearMineViewModel vm = new ClearMineViewModel();

        public ClearMineWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            vm.Start(9, 9, 10);
        }

        private void OnMineGroudMouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = e.ExtractDataContext<MineCell>();
            if (cell != null)
            {
                
            }
        }

        private void OnMineGroudMouseUp(object sender, MouseButtonEventArgs e)
        {
            var cell = e.ExtractDataContext<MineCell>();
            if (cell != null)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    vm.DigAt(cell);
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    vm.MarkAsMine(cell);
                }
            }
        }
    }
}
