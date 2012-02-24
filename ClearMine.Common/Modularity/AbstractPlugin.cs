namespace ClearMine.Common.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractPlugin : IPlugin
    {
        protected IList<PluginOption> pluginOptions = null;

        public string Name { get { return ResourceHelper.FindText(NameKey); } }

        public abstract string NameKey { get; }

        public string Description { get { return ResourceHelper.FindText(DescriptionKey); } }

        public abstract string DescriptionKey { get; }

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
            set { CurrentDataMap["IsEnabled"] = value; }
        }

        public virtual IEnumerable<PluginOption> Options
        {
            get
            {
                if (pluginOptions == null)
                {
                    if (CurrentDataMap["Options"] == null)
                    {
                        pluginOptions = new List<PluginOption>();
                        InitializeOptions();
                        CurrentDataMap["Options"] = pluginOptions;
                        Settings.Default.Save();
                    }
                    else
                    {
                        pluginOptions = CurrentDataMap["Options"] as IList<PluginOption>;
                    }
                }

                return pluginOptions;
            }
        }

        public virtual PluginOption this[string id]
        {
            get
            {
                if (pluginOptions != null)
                {
                    foreach (var option in pluginOptions)
                    {
                        if (String.CompareOrdinal(option.ID, id) == 0)
                        {
                            return option;
                        }
                    }
                }

                Trace.TraceError(String.Format("插件{0}没有初始化配置项{1}", Name, id));

                return null;
            }
        }

        public abstract void Initialize();

        protected abstract void InitializeOptions();
    }
}
