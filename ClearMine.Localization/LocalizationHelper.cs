namespace ClearMine.Localization
{
    using System.Windows;

    public static class LocalizationHelper
    {
        public static string FindText(object key)
        {
            return Application.Current.FindResource(key) as string;
        }
    }
}
