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

                if (map == null)
                {
                    map = Settings.Default.DataMap[Name] = new DataMap();
                }
                else
                {
                    Trace.Assert(map is DataMap, ResourceHelper.FindText("PluginOptionsTypeInvalid"));
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
                        if (String.CompareOrdinal(option.Id, id) == 0)
                        {
                            return option;
                        }
                    }
                }

                Trace.TraceError(ResourceHelper.FindText("InvalidPluginOptionId", Name, id));

                return null;
            }
        }

        public abstract void Initialize();

        protected abstract void InitializeOptions();
    }
}
