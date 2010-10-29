namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using ClearMine.Framework.ComponentModel;

    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    internal partial class StatisticsWindow : Window
    {
        private ViewModelBase vm = new StatisticsViewModel();

        public StatisticsWindow()
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
