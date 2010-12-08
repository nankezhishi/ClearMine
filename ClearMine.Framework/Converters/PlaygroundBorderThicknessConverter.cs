namespace ClearMine.Framework.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class PlaygroundBorderThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            if (values == null || values.Length < 2)
            {
                throw new ArgumentException("values must contains at least two element.", "values");
            }

            double width = (double)values[0];
            int columns = (int)values[1];
            double itemPercent = Double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);

            if (columns > 0)
            {
                return new Thickness(itemPercent * width / (columns + itemPercent * 2));
            }
            else
            {
                return new Thickness();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
