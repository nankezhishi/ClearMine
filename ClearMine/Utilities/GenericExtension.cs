namespace ClearMine.Utilities
{
    using System.Reflection;
    using System.Windows;

    internal static class GenericExtension
    {
        private const string propertySetterPrefix = "set_";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsPropertySetter(this MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith(propertySetterPrefix);
        }

        public static T ExtractDataContext<T>(this RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement)
            {
                return (T)(args.OriginalSource as FrameworkElement).DataContext;
            }
            else if (args.OriginalSource is FrameworkContentElement)
            {
                return (T)(args.OriginalSource as FrameworkContentElement).DataContext;
            }
            else
            {
                return default(T);
            }
        }
    }
}
