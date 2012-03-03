namespace ClearMine.Framework.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class ThumbExt
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.RegisterAttached("ResizeMode", typeof(ResizeDirection), typeof(ThumbExt), new PropertyMetadata(new PropertyChangedCallback(OnResizeModePropertyChanged)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResizeTargetProperty = DependencyProperty.RegisterAttached("ResizeTarget", typeof(FrameworkElement), typeof(ThumbExt), new PropertyMetadata(new PropertyChangedCallback(OnResizeTargetPropertyChanged)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ResizeDirection GetResizeMode(DependencyObject element)
        {
            return (ResizeDirection)element.GetValue(ResizeModeProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetResizeMode(DependencyObject element, ResizeDirection value)
        {
            element.SetValue(ResizeModeProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static FrameworkElement GetResizeTarget(DependencyObject element)
        {
            return element.GetValue(ResizeTargetProperty) as FrameworkElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetResizeTarget(DependencyObject element, FrameworkElement value)
        {
            element.SetValue(ResizeTargetProperty, value);
        }

        private static void OnResizeModePropertyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            if (!(element is Thumb))
                throw new InvalidOperationException("The ResizeMode Property should only used on Thumb.");

            (element as Thumb).DragDelta += new DragDeltaEventHandler(OnResizeThumbDragDelta);
        }

        private static void OnResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement resizeTarget = GetResizeTarget(sender as DependencyObject);

            #region Guards

            if (resizeTarget == null)
                throw new InvalidOperationException("ResizeTarget is required on the Thumb");

            #endregion

            ResizeDirection resizeMode = GetResizeMode(sender as DependencyObject);
            switch (resizeMode)
            {
                case ResizeDirection.RightDown:
                    {
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, e.HorizontalChange);
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, e.VerticalChange);
                        break;
                    }
                case ResizeDirection.RightUp:
                    {
                        resizeTarget.MovePosition(0, e.VerticalChange);
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, e.HorizontalChange);
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, -e.VerticalChange);
                        break;
                    }
                case ResizeDirection.LeftDown:
                    {
                        resizeTarget.MovePosition(e.HorizontalChange, 0);
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, -e.HorizontalChange);
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, e.VerticalChange);
                        break;
                    }
                case ResizeDirection.LeftUp:
                    {
                        resizeTarget.MovePosition(e.HorizontalChange, e.VerticalChange);
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, -e.HorizontalChange);
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, -e.VerticalChange);
                        break;
                    }
                case ResizeDirection.HorizontalRight:
                    {
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, e.HorizontalChange); break;
                    }
                case ResizeDirection.HorizontalLeft:
                    {
                        resizeTarget.MovePosition(e.HorizontalChange, 0);
                        resizeTarget.Width = AdjustWidthByChange(resizeTarget, -e.HorizontalChange); break;
                    }
                case ResizeDirection.VerticalDown:
                    {
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, e.VerticalChange); break;
                    }
                case ResizeDirection.VerticalUp:
                    {
                        resizeTarget.MovePosition(0, e.VerticalChange);
                        resizeTarget.Height = AdjustHeightByChange(resizeTarget, -e.VerticalChange); break;
                    }
            }
        }

        private static void OnResizeTargetPropertyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            if (!(element is Thumb))
                throw new InvalidOperationException("The ResizeMode Property should only used on Thumb.");

            if (e.NewValue == null)
            {
                (element as Thumb).IsEnabled = false;
                (element as Thumb).IsHitTestVisible = false;
            }
            else
            {
                (element as Thumb).IsEnabled = true;
                (element as Thumb).IsHitTestVisible = true;
            }
        }

        private static double AdjustWidthByChange(FrameworkElement element, double horizontalChange)
        {
            if (element.ActualWidth + horizontalChange > element.MinWidth)
                return element.ActualWidth + horizontalChange;
            else
                return element.MinWidth;
        }

        private static double AdjustHeightByChange(FrameworkElement element, double verticalChange)
        {
            if (element.ActualHeight + verticalChange > element.MinHeight)
                return element.ActualHeight + verticalChange;
            else
                return element.MinHeight;
        }
    }
}
