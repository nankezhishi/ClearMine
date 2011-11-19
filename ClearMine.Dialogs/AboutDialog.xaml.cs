namespace ClearMine.UI.Dialogs
{
    using System.Windows;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    internal partial class AboutDialog
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void OnDonateButtonClick(object sender, RoutedEventArgs e)
        {
            WebTools.Donate(10M, "guqiangqiang@gmail.com");
        }
    }
}
