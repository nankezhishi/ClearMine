namespace ClearMine.UI.Dialogs
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    internal partial class OptionsDialog : Window
    {
        private OptionsViewModel vm = new OptionsViewModel();

        public OptionsDialog()
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
