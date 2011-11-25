namespace ClearMine.Common.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public static class ModularityManager
    {
        private static IList<IModule> loadedModules = new List<IModule>();
        private static IList<IPlugin> loadedPlugins = new List<IPlugin>();

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<IPlugin> LoadedPlugins
        {
            get { return loadedPlugins; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<IModule> LoadedModules
        {
            get { return loadedModules; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadModules()
        {
            var dlls = Directory.EnumerateFiles(".", "*.dll", SearchOption.AllDirectories).Concat(Directory.EnumerateFiles(".", "*.exe", SearchOption.AllDirectories));
            foreach(var dll in dlls)
            {
                try
                {
                    var assembly = Assembly.LoadFile(Path.GetFullPath(dll));
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetInterface(typeof(IModule).FullName) != null
                            && loadedModules.All(m => m.GetType() != type))
                        {
                            var module = Activator.CreateInstance(type) as IModule;
                            module.InitializeModule();
                            loadedModules.Add(module);
                        }
                        else if (type.GetInterface(typeof(IPlugin).FullName) != null
                            && loadedPlugins.All(m => m.GetType() != type))
                        {
                            var plugin = Activator.CreateInstance(type) as IPlugin;
                            plugin.Initialize();
                            loadedPlugins.Add(plugin);
                        }
                        else
                        {
                            // Do nothing.
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }
    }
}
