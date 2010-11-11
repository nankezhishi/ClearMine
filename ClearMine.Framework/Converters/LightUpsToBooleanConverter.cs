namespace ClearMine.Framework.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using ClearMine.Framework.Controls;

    [ValueConversion(typeof(LightUps), typeof(bool))]
    public class LightUpsToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LightUps target = (LightUps)Enum.Parse(typeof(LightUps), parameter.ToString());
            LightUps flags = (LightUps)value;
            return (flags & target) == target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
