namespace ClearMine.Framework.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal class MineCellControl : Control
    {
        static MineCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MineCellControl), new FrameworkPropertyMetadata(typeof(MineCellControl)));
        }

        #region Mark Property
        /// <summary>
        /// Gets or sets the Mark property of current instance of MineCellControl
        /// </summary>
        public Brush Mark
        {
            get { return (Brush)GetValue(MarkProperty); }
            set { SetValue(MarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register("Mark", typeof(Brush), typeof(MineCellControl), new UIPropertyMetadata(new PropertyChangedCallback(OnMarkPropertyChanged)));

        private static void OnMarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MineCellControl instance = sender as MineCellControl;
            if (instance != null)
            {
                instance.OnMarkChanged(e);
            }
        }

        protected virtual void OnMarkChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
