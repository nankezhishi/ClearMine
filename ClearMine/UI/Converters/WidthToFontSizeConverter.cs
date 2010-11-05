using System;
using System.Globalization;
using System.Windows.Data;

namespace ClearMine.UI.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    internal class WidthToFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 0.75;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
