namespace ClearMine.Framework.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Adorner that holds a UIElement as its content. Must be used in code.
    /// </summary>
    /// <remarks>
    /// Everyone would like to use the Adorner like a control.
    /// This VisualAdorner Helps you make it!
    /// </remarks>
    [ContentProperty("Child")]
    [DefaultProperty("Child")]
    public class VisualAdorner : Adorner
    {
        /// <summary>
        /// Default constructor of VisualAdorner.
        /// Initialize an instance of the VisualAdorner.
        /// </summary>
        /// <param name="adornedElement">UI element on which this adorner will be shown.</param>
        /// <param name="content">Content of the adorner you want to show.</param>
        public VisualAdorner(UIElement adornedElement, UIElement child = null)
            : base(adornedElement)
        {
            Child = child;
        }

        #region Child Property
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof(UIElement), typeof(VisualAdorner),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChildPropertyChanged)));

        /// <summary>
        /// Gets or sets the Child of the VisualAdorner
        /// Why not object type Content? 
        /// Put a ContentPresenter provide you that capability,
        /// it's easier to make this support object type content.
        /// </summary>
        [Bindable(true)]
        [DefaultValue(null)]
        public UIElement Child
        {
            get { return GetValue(ChildProperty) as UIElement; }
            set { SetValue(ChildProperty, value); }
        }

        protected virtual void OnChildChanged(DependencyPropertyChangedEventArgs e)
        {
            // Do nothing, for extension by overriding it.
        }

        /// <summary>
        /// Key statements to make sure the controls in this Adorner works.
        /// Make it not virtual to prevent being overrided.
        /// Not everyone knows they need to call base.OnContentPropertyChanged(e);
        /// </summary>
        private void OnChildPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                RemoveVisualChild(e.OldValue as Visual);
                RemoveLogicalChild(e.OldValue);
            }
            if (e.NewValue != null)
            {
                AddVisualChild(e.NewValue as Visual);
                AddLogicalChild(e.NewValue);
            }

            // Call a virtual method that gives user extensibility.
            OnChildChanged(e);
        }

        private static void OnChildPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender != null && sender is VisualAdorner)
            {
                (sender as VisualAdorner).OnChildPropertyChanged(e);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (VisualChildrenCount > 0)
            {
                var child = GetVisualChild(0) as UIElement;
                if (child != null)
                {
                    child.Arrange(new Rect(finalSize));
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Base class <c>Adorner</c> always throw exception.
        /// So we must override it.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index < VisualChildrenCount && index >= 0)
            {
                return Child;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Adorner returns 0, So we must override it.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return Child == null ? 0 : 1; }
        }
    }
}
