namespace ClearMine.Common.Utilities
{
    using System.ComponentModel;
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1810",
        Justification = "Can you tell me a way to initialize container inline?")]
    public static class Infrastructure
    {
        private static bool? isInDesignMode;
        // "." is different from Path.GetFullPath(".") in design time.
        private static CompositionContainer container = new CompositionContainer(new AggregateCatalog(new DirectoryCatalog(Path.GetFullPath("."))));

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                }

                return isInDesignMode.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static CompositionContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);
        }
    }
}
