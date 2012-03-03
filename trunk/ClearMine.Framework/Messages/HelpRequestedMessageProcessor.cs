namespace ClearMine.Framework.Messages
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    internal class HelpRequestedMessageProcessor
    {
        public void HandleMessage(HelpRequestedMessage message)
        {
            if (message == null)
                return;

            var helpName = Settings.Default.HelpDocumentName;

            if (!String.IsNullOrWhiteSpace(helpName) && helpName.EndsWith("chm", true, CultureInfo.InvariantCulture))
            {
                try
                {
                    Process.Start(helpName);
                }
                catch (FileNotFoundException)
                {
                    Trace.TraceError(ResourceHelper.FindText("CannotFindHelpFile", helpName));
                }
            }
            else
            {
                Trace.TraceError(ResourceHelper.FindText("InvalidHelpFileType", helpName));
            }
        }
    }
}
