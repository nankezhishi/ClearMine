namespace ClearMine.Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public static class EmailHelper
    {
        public static void Send(string message, string title, string to)
        {
            Process p = new Process();
            p.StartInfo.FileName = String.Format(CultureInfo.InvariantCulture, "mailto:{0}?subject={1}&body={2}", to, title, message);
            p.Start();
        }
    }
}
