namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using ClearMine.Framework.Dialogs;

    /// <summary>
    /// Interaction logic for GameLostWindow.xaml
    /// </summary>
    internal partial class GameLostWindow : OptionDialog
    {
        public GameLostWindow()
        {
            InitializeComponent();
        }

        private void OnNewGameButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnTryAgainClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
