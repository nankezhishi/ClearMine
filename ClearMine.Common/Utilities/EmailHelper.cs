namespace ClearMine.Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Send email via mailto URL schema introduced in the RFC2368.
    /// For more information, please refer to http://tools.ietf.org/html/rfc2368
    /// </summary>
    public static class EmailHelper
    {
        public static void Send(string message, string title, string to)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = String.Format(CultureInfo.InvariantCulture, "mailto:{0}?subject={1}&body={2}", to, title, message);
                p.Start();
            }
        }
    }
}
