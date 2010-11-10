namespace ClearMine.Common.Utilities
{
    using System.IO;
    using System.Reflection;

    public static class Util
    {
        public static string GetAbsolutePath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path);
        }
    }
}
