namespace ClearMine.Framework.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    [Flags]
    public enum LightUps
    {
        Top = 1,
        TopL = 2,
        TopR = 4,
        Middle = 8,
        Bottom = 16,
        BottomL = 32,
        BottomR = 64,
        Number0 = 111,
        Number1 = 68,
        Number2 = 61,
        Number3 = 93,
        Number4 = 78,
        Number5 = 91,
        Number6 = 123,
        Number7 = 69,
        Number8 = 127,
        Number9 = 95,
    }

    public class NumberPresenter : Control
    {
        static NumberPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberPresenter), new FrameworkPropertyMetadata(typeof(NumberPresenter)));
        }

        #region LightUpStrokes Property - ReadOnly
        /// <summary>
        /// Gets the LightUpStrokes property of current instance of NumberPresenter
        /// </summary>
        public LightUps LightUpStrokes
        {
            get { return (LightUps)GetValue(LightUpStrokesPropertyKey.DependencyProperty); }
            protected set { SetValue(LightUpStrokesPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for LightUpStrokes.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey LightUpStrokesPropertyKey =
            DependencyProperty.RegisterReadOnly("LightUpStrokes", typeof(LightUps), typeof(NumberPresenter), new UIPropertyMetadata(null));

        public static readonly DependencyProperty LightUpStrokesProperty = LightUpStrokesPropertyKey.DependencyProperty;
        #endregion

        #region Number Property
        /// <summary>
        /// Gets or sets the Number property of current instance of NumberPresenter
        /// </summary>
        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(NumberPresenter), new UIPropertyMetadata(new PropertyChangedCallback(OnNumberPropertyChanged)));

        private static void OnNumberPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumberPresenter instance = sender as NumberPresenter;
            if (instance != null)
            {
                instance.OnNumberChanged(e);
            }
        }

        protected virtual void OnNumberChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Number < 0 || Number > 10)
            {
                throw new NotSupportedException();
            }

            LightUpStrokes = (LightUps)Enum.Parse(typeof(LightUps), "Number" + Number);
        }
        #endregion
    }
}
