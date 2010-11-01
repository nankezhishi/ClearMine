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
    }
}
