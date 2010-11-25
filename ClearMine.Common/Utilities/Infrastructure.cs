namespace ClearMine.Common.Utilities
{
    using System.ComponentModel.Composition.Hosting;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;

    [SuppressMessage("Microsoft.Performance", "CA1810",
        Justification = "Can you tell me a way to initialize container inline?")]
    public static class Infrastructure
    {
        private static CompositionContainer container;

        static Infrastructure()
        {
            var catalog = new AggregateCatalog();
            var moduleDirectory = new DirectoryCatalog(@".");
            catalog.Catalogs.Add(moduleDirectory);
            container = new CompositionContainer(catalog);
        }

        public static CompositionContainer Container
        {
            get { return container; }
        }

        public static string GetAbsolutePath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);
        }
    }
}
