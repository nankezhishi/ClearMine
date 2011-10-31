namespace ClearMine.Framework.Converters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows.Data;
    using ClearMine.Common;

    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Used in Xaml.")]
    [ValueConversion(typeof(Difficulty?), typeof(bool))]
    public class DifficultyToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetName(typeof(Difficulty), value) == parameter as string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Enum.Parse(typeof(Difficulty), parameter as string);
            }
            else
            {
                return null;
            }
        }
    }
}
