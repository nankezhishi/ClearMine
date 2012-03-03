namespace ClearMine.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class SwitchLanguageMessage : MessageBase
    {
        private const string customLanguageKey = "Custom";

        public static string CustomLanguageKey
        {
            get { return customLanguageKey; }
        }

        public string CultureName { get; set; }

        public SwitchLanguageMessage(string cultureName)
        {
            CultureName = cultureName;
        }

        public SwitchLanguageMessage(string cultureName, object source)
            : this(cultureName)
        {
            Source = source;
        }
    }
}
