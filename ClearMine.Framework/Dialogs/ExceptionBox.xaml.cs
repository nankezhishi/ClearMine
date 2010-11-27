namespace ClearMine.Framework.Dialogs
{
    using System;
    using System.Windows;
    using ClearMine.Common.Utilities;
    using ClearMine.Localization;

    /// <summary>
    /// Interaction logic for ExceptionBox.xaml
    /// </summary>
    public partial class ExceptionBox : OptionDialog
    {
        private Exception exception;

        private ExceptionBox(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            this.exception = exception;
            Message = exception.Message;
            InitializeComponent();
        }

        public string StackTrace
        {
            get { return exception.StackTrace; }
        }

        public bool HasInnerException
        {
            get { return exception.InnerException != null; }
        }

        public static bool? Show(Exception exception, Window owner = null)
        {
            return new ExceptionBox(exception)
            {
                Owner = owner ?? Application.Current.MainWindow,
            }.ShowDialog();
        }

        private void OnShutdownClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnIgnoreClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnShowInnerException(object sender, RoutedEventArgs e)
        {
            DialogResult = Show(exception.InnerException, this);
        }

        private void OnReportButtonClick(object sender, RoutedEventArgs e)
        {
            EmailHelper.Send(exception.ToString(), LocalizationHelper.FindText("ExceptionReportTitle"), "nankezhishi@hotmail.com");
        }
    }
}
