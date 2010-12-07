namespace ClearMine.UI.Dialogs
{
    using System.Windows;

    using ClearMine.VM;

    /// <summary>
    /// Interaction logic for ClearMineWindow.xaml
    /// </summary>
    public partial class ClearMineWindow : Window
    {
        public ClearMineWindow()
        {
            this.DataContext = new ClearMineViewModel();
            InitializeComponent();
        }
    }
}
