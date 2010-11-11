namespace ClearMine.Framework.Controls
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// This panel make sure the width and height are equal.
    /// </summary>
    public class MinesPanel : UniformGrid
    {
        private Size measuredResult;

        static MinesPanel()
        {
            Grid.ShowGridLinesProperty.AddOwner(typeof(MinesPanel));
        }

        public MinesPanel()
        {
            ShowGridLines = true;
        }

        #region ShowGridLines Property
        /// <summary>
        /// Gets or sets the ShowGridLines property of current instance of MinesPanel
        /// </summary>
        public bool ShowGridLines
        {
            get { return (bool)GetValue(Grid.ShowGridLinesProperty); }
            set { SetValue(Grid.ShowGridLinesProperty, value); }
        }
        #endregion
        #region GridLineThickness Property
        /// <summary>
        /// Gets or sets the GridLineThickness property of current instance of MinesPanel
        /// </summary>
        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridLineThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register("GridLineThickness", typeof(double), typeof(MinesPanel), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnGridLineThicknessPropertyChanged)));

        private static void OnGridLineThicknessPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MinesPanel instance = sender as MinesPanel;
            if (instance != null)
            {
                instance.OnGridLineThicknessChanged(e);
            }
        }

        protected virtual void OnGridLineThicknessChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
        #region GridLineBrush Property
        /// <summary>
        /// Gets or sets the GridLineBrush property of current instance of MinesPanel
        /// </summary>
        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(MinesPanel), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnGridLineBrushPropertyChanged)));

        private static void OnGridLineBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MinesPanel instance = sender as MinesPanel;
            if (instance != null)
            {
                instance.OnGridLineBrushChanged(e);
            }
        }

        protected virtual void OnGridLineBrushChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
        #region ItemSize Property
        /// <summary>
        /// Gets the ItemSize property of current instance of MinesPanel
        /// </summary>
        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSize.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(double), typeof(MinesPanel), new UIPropertyMetadata(0.0));
        #endregion

        protected override void OnRender(DrawingContext dc)
        {
            if (ShowGridLines)
            {
                var pen = new Pen(GridLineBrush, GridLineThickness);

                double hInterval = this.DesiredSize.Width / Columns;
                for (double x = hInterval - GridLineThickness / 2; x < this.DesiredSize.Width; x += hInterval)
                {
                    dc.DrawLine(pen, new Point(x, 0), new Point(x, this.RenderSize.Height));
                }
                double vInterval = this.DesiredSize.Height / Rows;
                for (double y = vInterval - GridLineThickness / 2; y < this.DesiredSize.Height; y += vInterval)
                {
                    dc.DrawLine(pen, new Point(0, y), new Point(this.RenderSize.Width, y));
                }
            }

            base.OnRender(dc);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (Columns == 0 || Rows == 0)
            {
                return new Size();
            }

            measuredResult = base.MeasureOverride(constraint);

            double finalWidth = constraint.Width;
            double finalHeight = finalWidth * Rows / Columns;
            if (finalWidth > constraint.Height * Columns / Rows)
            {
                finalHeight = constraint.Height;
                finalWidth = finalHeight * Columns / Rows;
            }
            if (Double.IsInfinity(finalWidth) || Double.IsNaN(finalWidth))
            {
                return measuredResult;
            }

            ItemSize = (int)finalWidth / Columns;
            finalWidth = ItemSize * Columns;
            finalHeight = finalWidth * Rows / Columns;

            measuredResult = new Size(finalWidth, finalHeight);
            return measuredResult;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Trace.TraceInformation("MinesPanel Arranged.");

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
                    element.Arrange(new Rect());
                }
            }

            return arrangeSize;
        }
    }
}
