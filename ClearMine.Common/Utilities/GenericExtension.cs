namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    public static class GenericExtension
    {
        private const string propertySetterPrefix = "set_";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsPropertySetter(this MethodBase method)
        {
            if (method == null)
            {
                throw new InvalidProgramException();
            }

            return method.IsSpecialName && method.Name.StartsWith(propertySetterPrefix, StringComparison.Ordinal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object instance, string memberName)
        {
            var member = instance.GetType().GetProperty(memberName);
            if (member == null)
                throw new ArgumentException(ResourceHelper.FindText("CannotFoundProperty", instance.GetType().FullName, memberName));
            else
                return (T)member.GetValue(instance, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        public static void SetValue(this object instance, string memberName, object value)
        {
            var member = instance.GetType().GetProperty(memberName);
            if (member == null)
                throw new ArgumentException(ResourceHelper.FindText("CannotFoundProperty", instance.GetType().FullName, memberName));
            else
                member.SetValue(instance, value, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject element, Predicate<T> condition = null)
            where T : DependencyObject
        {
            var children = new List<T>();
            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as T;
                if (child != null)
                {
                    children.AddRange((condition ?? ((T c) => true))(child) ? new[] { child } : FindChildren<T>(child, condition));
                }
            }

            return children;
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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        public static void CloseParentWindow(this DependencyObject obj, bool? result)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var window = Window.GetWindow(obj);
            if (window != null)
            {
                window.DialogResult = result;
                window.Close();
            }
        }

        /// <summary>
        /// This method is rather heavy. it will takes 5K ticks, about 2ms on a 2.5G CPU.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<T>(Expression<Func<object>> expression)
        {
            return GetMemberName(expression);
        }

        /// <summary>
        /// This method is rather heavy. it will takes 5K ticks, about 2ms on a 2.5G CPU.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression as LambdaExpression);
        }

        /// <summary>
        /// Format a string in a culture invariant way.
        /// </summary>
        /// <param name="format">The string to format</param>
        /// <param name="args"></param>
        /// <returns>The formated string</returns>
        public static string InvariantFormat(this string format, params object[] args)
        {
            if (format == null)
                return null;

            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool IsAssignableToAny(this Type type, params Type[] types)
        {
            foreach (var t in types)
            {
                if (t.IsAssignableFrom(type))
                {
                    return true;
                }
            }

            return false;
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
                throw new ArgumentException(ResourceHelper.FindText("InvalidExpressType", expression.Body.Type));
            }

            return memberExpression.Member.Name;
        }

        [Conditional("DEBUG")]
        private static void VerifyFindAncestorCall<T>()
            where T : DependencyObject
        {
            if (typeof(T).IsSubclassOf(typeof(Window)))
            {
                throw new InvalidProgramException(ResourceHelper.FindText("UseWindowToGetWindow"));
            }
        }
    }
}
