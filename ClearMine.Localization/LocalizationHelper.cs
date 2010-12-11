namespace ClearMine.Localization
{
    using System;
    using System.Globalization;
    using System.Windows;

    public static class LocalizationHelper
    {
        public static string FindText(object key)
        {
            return Application.Current.FindResource(key) as string;
        }

        public static string FindText(object key, params string[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, FindText(key), args);
        }
    }
}
