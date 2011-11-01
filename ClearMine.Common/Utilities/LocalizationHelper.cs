namespace ClearMine.Common.Utilities
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

        public static string FindText(object key, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, FindText(key), args);
        }
    }
}
