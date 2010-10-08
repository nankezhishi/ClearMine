namespace ClearMine.Utilities
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

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
            return method.IsSpecialName && method.Name.StartsWith(propertySetterPrefix, StringComparison.Ordinal);
        }

        public static T FindAncestor<T>(this DependencyObject element, Predicate<T> condition = null)
            where T : DependencyObject
        {
            if (element == null)
            {
                return null;
            }

            T result = element as T;

            if (result != null && (condition ?? (e => true))(result))
            {
                return result;
            }

            var parent = VisualTreeHelper.GetParent(element);

            if (parent == null && (element is FrameworkElement || element is FrameworkContentElement))
            {
                parent = ((dynamic)element).Parent ?? ((dynamic)element).TemplatedParent ?? parent;
            }

            return parent.FindAncestor(condition);
        }

        public static T ExtractDataContext<T>(this RoutedEventArgs args)
        {
            var source = args.OriginalSource as DependencyObject;
            if (source != null)
            {
                dynamic element = source.FindAncestor((DependencyObject e) => ((dynamic)e).DataContext is T);

                if (element != null)
                {
                    return (T)element.DataContext;
                }
            }

            return default(T);
        }
    }
}
