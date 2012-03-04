namespace ClearMine.Dialogs
{
    using System.Windows;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    internal partial class AboutDialog
    {
        /// <summary>
        /// 
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void OnDonateButtonClick(object sender, RoutedEventArgs e)
        {
            WebTools.Donate("fadefy@gmail.com", 10M);
        }
    }
}
