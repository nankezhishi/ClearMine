namespace ClearMine.Framework.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    internal class AutoTransformPanel : Panel
    {
        private double maxWidth = -1;
        private double maxHeight = -1;
        private bool pendingArrange = true;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (maxWidth < 0 || maxHeight < 0)
            {
                foreach (UIElement child in Children)
                {
                    child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                }

                var sizes = from child in Children.Cast<UIElement>() select child.DesiredSize;
                maxWidth = (from size in sizes select size.Width).Max();
                maxHeight = (from size in sizes select size.Height).Max();
            }

            return new Size(maxWidth, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                if (pendingArrange)
                {
                    child.Arrange(new Rect(child.DesiredSize));
                }

                var transform = GetTransformFromChild(child);
                if (finalSize.Width < child.DesiredSize.Width)
                {
                    transform.ScaleX = finalSize.Width / child.DesiredSize.Width;
                }
                if (finalSize.Height < child.DesiredSize.Height)
                {
                    transform.ScaleY = finalSize.Height / child.DesiredSize.Height;
                }
            }

            pendingArrange = false;

            return finalSize;
        }

        private static ScaleTransform GetTransformFromChild(UIElement child)
        {
            ScaleTransform scaleTransform = null;
            if (child.RenderTransform == null || child.RenderTransform == MatrixTransform.Identity)
            {
                scaleTransform = new ScaleTransform();
                child.RenderTransform = scaleTransform;
            }
            else
            {
                if (child.RenderTransform is ScaleTransform)
                {
                    scaleTransform = child.RenderTransform as ScaleTransform;
                }
                else if (child.RenderTransform is TransformGroup)
                {
                    foreach (Transform transfrom in (child.RenderTransform as TransformGroup).Children)
                    {
                        if (transfrom is ScaleTransform)
                        {
                            scaleTransform = transfrom as ScaleTransform;
                            break;
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return scaleTransform;
        }
    }
}
