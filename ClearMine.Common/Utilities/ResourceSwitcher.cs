namespace ClearMine.Common.Utilities
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using Microsoft.Win32;

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
        /// 当resourceString为空时，表示不对当前资源做任何改变。所以函数是成功完成的，所以要返回false。表示没有问题。
        /// </summary>
        /// <param name="resourceString"></param>
        /// <returns></returns>
        protected virtual bool SwitchResource(string resourceString)
        {
            if (resourceString == null)
                return false;

            try
            {
                var newDictionary = resourceString.MakeResDic();
                if (Resources[resourceIndex].VerifyResources(newDictionary, validResourceTypes))
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
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            resourceIndex = Resources.Count - 1;
        }
    }
}
