namespace ClearMine.Framework.Utilities
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
                return default(T);
            }

            T result = element as T;
            if (result != null && (condition == null || condition(result)))
            {
                return result;
            }

            // Just keep the code clean. 
            // While, .NET Reflector 6.5 still cannot handle the following code correctly.
            var parent = VisualTreeHelper.GetParent(element) ?? ((dynamic)element).Parent ?? ((dynamic)element).TemplatedParent;

            return parent.FindAncestor(condition);
        }

        public static T ExtractDataContext<T>(this RoutedEventArgs args, Action<T> action = null)
        {
            var source = args.OriginalSource as DependencyObject;
            if (source != null)
            {
                // Converting the element to dynamic type use more memory (About 5M/100 cells) but keep the code clean.
                // Otherwise we have to take care of Both FrameworkElement and FrameworkContentElement.
                dynamic element = source.FindAncestor((DependencyObject e) => ((dynamic)e).DataContext is T);

                if (element != null)
                {
                    T result = (T)element.DataContext;
                    if (action != null)
                    {
                        action(result);
                    }
                    return result;
                }
            }

            return default(T);
        }
    }
}
