namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using ClearMine.Framework.Dialogs;

    /// <summary>
    /// Interaction logic for GameWonWindow.xaml
    /// </summary>
    internal partial class GameWonWindow : OptionDialog
    {
        /// <summary>
        /// 
        /// </summary>
        public GameWonWindow()
        {
            InitializeComponent();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnNewGameButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
