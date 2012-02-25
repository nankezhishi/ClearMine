namespace ClearMine.Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    public static class ResourceHelper
    {
        public static string FindText(object key)
        {
            try
            {
                return Application.Current.FindResource(key) as string;
            }
            catch (ResourceReferenceKeyNotFoundException e)
            {
                Trace.TraceError(e.ToString());

                // Prevent the same error while reporting the error.
                if (!"CannotFoundResourceKey".Equals(key))
                {
                    Trace.TraceError(FindText("CannotFoundResourceKey", key));
                }

                return null;
            }
        }

        public static string FindText(object key, params object[] args)
        {
            return FindText(key).InvariantFormat(args);
        }

        /// <summary>
        /// Create an instance of ResourceDictionary from a string.
        /// </summary>
        /// <param name="key">The uri string of the resource.</param>
        /// <param name="args">The parameters to format the uri if any.</param>
        /// <returns>A new instance of ResourceDictionary</returns>
        public static ResourceDictionary MakeResource(this string key, params object[] args)
        {
            return new ResourceDictionary()
            {
                Source = new Uri(key.InvariantFormat(args), UriKind.RelativeOrAbsolute),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existing"></param>
        /// <param name="newResource"></param>
        /// <param name="validTypes"></param>
        /// <returns></returns>
        public static bool VerifyResources(this ResourceDictionary existing, ResourceDictionary newResource, params Type[] validTypes)
        {
            if (existing == null)
                throw new ArgumentNullException("existing");

            if (newResource == null)
                return false;

            if (validTypes != null && validTypes.Length > 0)
            {
                foreach (var resource in newResource.Values)
                {
                    if (!(resource.GetType().IsAssignableToAny(validTypes)))
                    {
                        MessageBox.Show(FindText("InvalidLanguageResourceType"), FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }

            var newKeys = newResource.Keys.Cast<object>();

            foreach (var existingKey in existing.Keys)
            {
                if (!newKeys.Contains(existingKey))
                {
                    var message = FindText("MissingLanguageResourceKey", existingKey);
                    MessageBox.Show(message, FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
