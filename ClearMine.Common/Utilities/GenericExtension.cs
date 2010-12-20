namespace ClearMine.Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
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
            VerifyFindAncestorCall<T>();

            if (element == null)
            {
                return default(T);
            }

            T result = element as T;
            if (result != null && (condition == null || condition(result)))
            {
                return result;
            }

            // Just keep the code clean. Don't need to worry about performance here.
            // While, .NET Reflector 6.5 still cannot handle the following code correctly.
            var parent = VisualTreeHelper.GetParent(element) ?? ((dynamic)element).Parent ?? ((dynamic)element).TemplatedParent;

            // dynamic object don't support extension method. So we must call it in a static way.
            return FindAncestor(parent, condition);
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

        /// <summary>
        /// This method is rather heavy. it will takes 5K ticks, about 2ms on a 2.5G CPU.
        /// </summary>
        public static string GetMemberName<T>(this T obj, Expression<Func<object>> expression)
        {
            return GetMemberName(expression);
        }

        /// <summary>
        /// This method is rather heavy. it will takes 5K ticks, about 2ms on a 2.5G CPU.
        /// </summary>
        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression as LambdaExpression);
        }

        /// <summary>
        /// This method takes 18 CPU ticks.
        /// </summary>
        private static string GetMemberName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null && expression.Body is UnaryExpression)
            {
                memberExpression = (expression.Body as UnaryExpression).Operand as MemberExpression;
            }
            if (memberExpression == null)
            {
                throw new ArgumentException(String.Format("Expression of type {0} is not supported by the GetMemberName method.", expression.Body.Type));
            }

            return memberExpression.Member.Name;
        }

        [Conditional("DEBUG")]
        private static void VerifyFindAncestorCall<T>()
            where T : DependencyObject
        {
            if (typeof(T).IsSubclassOf(typeof(Window)))
            {
                throw new InvalidProgramException("Use Window.GetWindow method to find the ancestor window of a control.");
            }
        }
    }
}
