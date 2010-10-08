namespace ClearMine.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    internal class MineCellControl : Control
    {
        static MineCellControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MineCellControl), new FrameworkPropertyMetadata(typeof(MineCellControl)));
        }
    }
}
