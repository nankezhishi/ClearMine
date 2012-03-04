namespace ClearMine.Common.Utilities
{
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    public static class WebTools
    {
        /// <summary>
        /// Send email via mailto URL schema introduced in the RFC2368.
        /// For more information, please refer to http://tools.ietf.org/html/rfc2368
        /// </summary>
        public static void SendEmail(string message, string title, string to)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = "mailto:{0}?subject={1}&body={2}".InvariantFormat(to, title, message);
                p.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="to"></param>
        public static void Donate(string to, decimal amount, string currencyCode = "CNY")
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = "https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business={0}&currency_code={1}&amount={2}&item_name={3}".InvariantFormat(to, currencyCode, amount, "Donate Clear Mine");
                p.Start();
            }
        }
    }
}
