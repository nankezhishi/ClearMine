﻿namespace ClearMine.Framework.Controls
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class NumberPresenter : Control
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static NumberPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberPresenter), new FrameworkPropertyMetadata(typeof(NumberPresenter)));
        }

        public NumberPresenter()
        {
            Trace.TraceInformation(ResourceHelper.FindText("NewNumberPresenterCreated"));
        }

        #region LightUpStrokes Property - ReadOnly
        /// <summary>
        /// Gets the LightUpStrokes property of current instance of NumberPresenter
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpStrokes")]
        public LightUps LightUpStrokes
        {
            get { return (LightUps)GetValue(LightUpStrokesPropertyKey.DependencyProperty); }
            protected set { SetValue(LightUpStrokesPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for LightUpStrokes.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey LightUpStrokesPropertyKey =
            DependencyProperty.RegisterReadOnly("LightUpStrokes", typeof(LightUps), typeof(NumberPresenter), new UIPropertyMetadata(null));

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "UpStrokes")]
        public static readonly DependencyProperty LightUpStrokesProperty = LightUpStrokesPropertyKey.DependencyProperty;
        #endregion

        #region Number Property
        /// <summary>
        /// Gets or sets the Number property of current instance of NumberPresenter
        /// </summary>
        public object Number
        {
            get { return (object)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(object), typeof(NumberPresenter), new UIPropertyMetadata(new PropertyChangedCallback(OnNumberPropertyChanged)));

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
            int number = Convert.ToInt32(e.NewValue, CultureInfo.InvariantCulture);

            if (number >= '0' && number <= '9')
            {
                number -= '0';
                LightUpStrokes = (LightUps)Enum.Parse(typeof(LightUps), "Number" + number.ToString(CultureInfo.InvariantCulture));
            }
            else if (number == '-')
            {
                LightUpStrokes = LightUps.Middle;
            }
            else if (number >= 0 && number <= 9)
            {
                LightUpStrokes = (LightUps)Enum.Parse(typeof(LightUps), "Number" + number.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                LightUpStrokes = 0;
            }
        }
        #endregion

        #region IsFloatingPoint Property
        /// <summary>
        /// Gets or sets the IsFloatingPoint property of current instance of NumberPresenter
        /// </summary>
        public bool IsFloatingPoint
        {
            get { return (bool)GetValue(IsFloatingPointProperty); }
            set { SetValue(IsFloatingPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFloatingPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFloatingPointProperty =
            DependencyProperty.Register("IsFloatingPoint", typeof(bool), typeof(NumberPresenter), new UIPropertyMetadata(new PropertyChangedCallback(OnIsFloatingPointPropertyChanged)));

        private static void OnIsFloatingPointPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumberPresenter instance = sender as NumberPresenter;
            if (instance != null)
            {
                instance.OnIsFloatingPointChanged(e);
            }
        }

        protected virtual void OnIsFloatingPointChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
