namespace ClearMine.Framework.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using ClearMine.Common.Utilities;

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
                throw new ArgumentException(ResourceHelper.FindText("RequiresTwoValues"), "values");
            }

            if (DependencyProperty.UnsetValue == values[0] || DependencyProperty.UnsetValue == values[1])
                return DependencyProperty.UnsetValue;

            try
            {
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
            catch (InvalidCastException e)
            {
                Trace.TraceError(e.ToString());
                Trace.TraceError(String.Format("Values are: {0}, {1}", values[0], values[1]));

                return 1;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
