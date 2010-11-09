namespace ClearMine.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    internal class MineCellControl : Control
    {
        static MineCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MineCellControl), new FrameworkPropertyMetadata(typeof(MineCellControl)));
        }

        #region IsPressed Property
        /// <summary>
        /// Gets or sets the IsPressed property of current instance of MineCellControl.
        /// Logically, this isPressed state is a pure UI state.
        /// </summary>
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register("IsPressed", typeof(bool), typeof(MineCellControl), new UIPropertyMetadata(new PropertyChangedCallback(OnIsPressedPropertyChanged)));

        private static void OnIsPressedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MineCellControl instance = sender as MineCellControl;
            if (instance != null)
            {
                instance.OnIsPressedChanged(e);
            }
        }

        protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            SetCurrentValue(MineCellControl.IsPressedProperty, true);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            SetCurrentValue(MineCellControl.IsPressedProperty, false);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            SetCurrentValue(MineCellControl.IsPressedProperty, false);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SetCurrentValue(MineCellControl.IsPressedProperty, true);
            }
        }
    }
}
