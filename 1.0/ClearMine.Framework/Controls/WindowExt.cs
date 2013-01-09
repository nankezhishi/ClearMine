namespace ClearMine.Framework.Controls
{
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public class WindowExt
    {
        public static ResizeMode GetResizeMode(DependencyObject obj)
        {
            return (ResizeMode)obj.GetValue(ResizeModeProperty);
        }

        public static void SetResizeMode(DependencyObject obj, ResizeMode value)
        {
            obj.SetValue(ResizeModeProperty, value);
        }

        /// <summary>
        /// The original ResizeMode property of Window must be set to NoResize if you want to custom window UI.
        /// So, we need another one to control the Resize Behavior of the customed window. Any other solution?
        /// </summary>
        public static readonly DependencyProperty ResizeModeProperty =
            DependencyProperty.RegisterAttached("ResizeMode", typeof(ResizeMode), typeof(WindowExt), new UIPropertyMetadata(ResizeMode.CanResize));
    }
}
