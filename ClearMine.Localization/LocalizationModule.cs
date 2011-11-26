namespace ClearMine.Localization
{
    using System.Windows.Media;

    using ClearMine.Common.Modularity;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizationModule : IModule
    {
        private LanguageSwitcher switcher;

        public void InitializeModule()
        {
            switcher=new LanguageSwitcher("/ClearMine.Localization;component/{0}/Overall.xaml", new[] { typeof(string), typeof(ImageSource) }, true);
        }
    }
}
