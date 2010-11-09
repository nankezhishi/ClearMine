namespace ClearMine.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class MathOperationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var operation = parameter.ToString();
            if (operation.StartsWith("*", StringComparison.Ordinal))
            {
                if ((double)value != 0.0)
                {
                    return (double)value * Double.Parse(operation.Substring(1));
                }
                else
                {
                    // Make the Fallback Value work.
                    return DependencyProperty.UnsetValue;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
