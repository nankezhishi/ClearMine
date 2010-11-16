namespace ClearMine.Common.Utilities
{
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Reflection;
    using System.ComponentModel.Composition.Primitives;

    public static class Util
    {
        private static CompositionContainer container;

        static Util()
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
