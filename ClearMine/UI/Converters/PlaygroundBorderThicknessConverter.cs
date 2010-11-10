namespace ClearMine.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class PlaygroundBorderThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)values[0];
            int columns = (int)values[1];
            double itemPercent = Double.Parse(parameter.ToString());

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
