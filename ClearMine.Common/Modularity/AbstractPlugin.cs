namespace ClearMine.Common.Modularity
{
    using System;
    using System.Collections.Generic;
    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractPlugin : IPlugin
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public virtual DataMap CurrentDataMap
        {
            get
            {
                var map = Settings.Default.DataMap[Name];
                if (map != null && !(map is DataMap))
                    throw new InvalidProgramException("插件的参数值应该为DataMap类型");

                if (map == null)
                {
                    map = Settings.Default.DataMap[Name] = new DataMap();
                }

                return map as DataMap;
            }
        }

        public bool IsEnabled
        {
            get { return (bool)(CurrentDataMap["IsEnabled"] ?? (CurrentDataMap["IsEnabled"] = false)); }
            set
            {
                CurrentDataMap["IsEnabled"] = value;
                Settings.Default.Save();
            }
        }

        public abstract IEnumerable<PluginOption> Options { get; }

        public abstract void Initialize();
    }
}
