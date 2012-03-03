namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceSwitcher
    {
        private int resourceIndex;
        private Type[] validResourceTypes;
        private bool supportCustom;
        private string resourceFormat;

        public static Collection<ResourceDictionary> Resources
        {
            get { return Application.Current.Resources.MergedDictionaries; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public string ResourceFormat
        {
            get { return resourceFormat; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public bool SupportCustom
        {
            get { return supportCustom; }
        }

        public event EventHandler<GenericEventArgs<Collection<ResourceDictionary>>> Initialized;

        public ResourceSwitcher(string resourceFormat, Type[] validTypes, bool supportCustom)
        {
            this.validResourceTypes = validTypes;
            this.resourceFormat = resourceFormat;
            this.supportCustom = supportCustom;
            Application.Current.Startup += new StartupEventHandler(CurrentApplicationStartup);
        }

        protected virtual string ShowUpOpenResourceDialog(string filterKey)
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".xaml",
                CheckFileExists = true,
                Multiselect = false,
                Filter = ResourceHelper.FindText(filterKey),
            };
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 当resourcePath为空时，表示不对当前资源做任何改变。所以函数是成功完成的，所以要返回false。表示没有问题。
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        protected virtual bool SwitchResource(string resourcePath)
        {
            if (resourcePath == null)
                return false;

            try
            {
                var newDictionary = resourcePath.MakeResource();
                // The custom resource initialization failed would cause current Resource count incorrect.
                if (Resources.Count <= resourceIndex)
                {
                    Trace.TraceError(String.Format("资源没有正确初始化成功"));

                    return true;
                }
                else if (Resources[resourceIndex].VerifyResources(newDictionary, validResourceTypes))
                {
                    Resources[resourceIndex] = newDictionary;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                var msg = ResourceHelper.FindText("ResourceParseError", ex.Message);
                MessageBox.Show(msg, ResourceHelper.FindText("ApplicationTitle"), MessageBoxButton.OK, MessageBoxImage.Error);

                return true;
            }

            return false;
        }

        protected virtual void OnApplicationStartup()
        {
        }

        private void CurrentApplicationStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Startup -= new StartupEventHandler(CurrentApplicationStartup);

            try
            {
                OnApplicationStartup();
                var temp = Initialized;
                if (temp != null)
                {
                    temp(this, new GenericEventArgs<Collection<ResourceDictionary>>(Resources));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            resourceIndex = Resources.Count - 1;
        }
    }
}
