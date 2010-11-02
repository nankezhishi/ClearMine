namespace ClearMine.UI.Dialogs
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    internal partial class StatisticsWindow : Window
    {
        public StatisticsWindow(Difficulty selectedLevel)
        {
            if (selectedLevel == Difficulty.Custom)
            {
                selectedLevel = Difficulty.Beginner;
            }
            DataContext = new StatisticsViewModel() { SelectedLevel = selectedLevel };
            InitializeComponent();
        }

        private void OnHistoryListLoaded(object sender, RoutedEventArgs e)
        {
            var control = (sender as FrameworkElement);
            if (control != null)
            {
                control.Width = control.ActualWidth;
            }
        }
    }
}
