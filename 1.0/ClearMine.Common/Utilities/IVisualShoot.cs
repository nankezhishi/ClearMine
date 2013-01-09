namespace ClearMine.Common.Utilities
{
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public interface IVisualShoot
    {
        void SaveSnapshoot(FrameworkElement element, string imageFilePath);

        string SaveSnapshoot(FrameworkElement element);
    }
}
