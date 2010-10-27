namespace ClearMine.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// This panel make sure the width and height are equal.
    /// </summary>
    public class MinesPanel : UniformGrid
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (this.Columns == 0 || this.Rows == 0)
            {
                return new Size(0, 0);
            }

            // make sure the child is a Square in its Shape.
            double itemWidth = arrangeSize.Width / ((double)this.Columns);
            double itemHeight = arrangeSize.Height / ((double)this.Rows);
            double itemSize = Math.Min(itemWidth, itemHeight);
            double beginX = 0.0;
            double beginY = 0.0;
            if (itemWidth < itemHeight)
            {
                beginY = (arrangeSize.Height - itemSize * this.Rows) / 2;
            }
            else if (itemWidth > itemHeight)
            {
                beginX = (arrangeSize.Width - itemSize * this.Columns) / 2;
            }

            Rect finalRect = new Rect(beginX, beginY, itemSize, itemSize);
            int row = 1;
            double width = finalRect.Width;
            double num2 = itemSize * this.Columns - 1.0 + beginX;
            finalRect.X += finalRect.Width * this.FirstColumn;
            foreach (UIElement element in base.InternalChildren)
            {
                if (row <= Rows)
                {
                    element.Arrange(finalRect);
                    finalRect.X += width;
                    if (finalRect.X >= num2)
                    {
                        ++row;
                        finalRect.Y += finalRect.Height;
                        finalRect.X = beginX;
                    }
                }
                else
                {
                    // Hide the redundent items.
                    element.Arrange(new Rect());
                }
            }

            return arrangeSize;
        }
    }
}
