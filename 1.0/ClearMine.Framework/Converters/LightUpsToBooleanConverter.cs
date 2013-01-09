namespace ClearMine.Framework.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using ClearMine.Framework.Controls;

    public class LightUpsToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return false;

            LightUps target = (LightUps)values[1];
            LightUps flags = (LightUps)values[0];
            return (flags & target) == target;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
