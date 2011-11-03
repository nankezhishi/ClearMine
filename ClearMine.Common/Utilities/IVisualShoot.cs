namespace ClearMine.Common.Utilities
{
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public interface IVisualShoot
    {
        void SaveSnapShoot(FrameworkElement element, string imageFilePath);

        string SaveSnapShoot(FrameworkElement element);
    }
}
