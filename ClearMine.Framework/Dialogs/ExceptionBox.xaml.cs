namespace ClearMine.Framework.Dialogs
{
    using System;
    using System.Windows;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// Interaction logic for ExceptionBox.xaml
    /// </summary>
    internal partial class ExceptionBox : OptionDialog
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
            try
            {
                return new ExceptionBox(exception)
                {
                    Owner = owner ?? Application.Current.MainWindow,
                }.ShowDialog();
            }
            // This happens when exception was throwed before the main window shows.
            catch (InvalidOperationException)
            {
                return new ExceptionBox(exception).ShowDialog();
            }
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
            WebTools.SendEmail(exception.ToString(), ResourceHelper.FindText("ExceptionReportTitle"), Settings.Default.FeedBackEmail);
        }
    }
}
