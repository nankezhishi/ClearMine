namespace ClearMine.Dialogs
{
    using System.Windows;
    using ClearMine.Framework.Dialogs;

    /// <summary>
    /// Interaction logic for ConfirmNewGameWindow.xaml
    /// </summary>
    internal partial class ConfirmNewGameWindow : OptionDialog
    {
        public ConfirmNewGameWindow()
        {
            InitializeComponent();
        }

        private void OnContinueClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnNewGameButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
