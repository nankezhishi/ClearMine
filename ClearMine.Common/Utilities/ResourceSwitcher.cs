namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceSwitcher
    {
        protected int resourceIndex;
        protected string resourceStringFormat;
        protected Type[] validResourceTypes;
        protected bool supportCustom;

        protected Collection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }

        public ResourceSwitcher(string stringFormat, Type[] validTypes, bool supportCustom)
        {
            this.validResourceTypes = validTypes;
            this.resourceStringFormat = stringFormat;
            this.supportCustom = supportCustom;
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
        }

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);

            try
            {
                OnApplicationStartup();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            resourceIndex = Resources.Count - 1;
        }

        protected virtual void OnApplicationStartup()
        {
        }
    }
}
