namespace ClearMine.Framework.Messages
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;

    internal class HelpRequestedMessageProcessor
    {
        public void HandleMessage(HelpRequestedMessage message)
        {
            var helpName = Settings.Default.HelpDocumentName;

            if (!String.IsNullOrWhiteSpace(helpName) && helpName.EndsWith("chm"))
            {
                try
                {
                    Process.Start(helpName);
                }
                catch (FileNotFoundException)
                {
                    Trace.TraceError(LocalizationHelper.FindText("CannotFindHelpFile", helpName));
                }
            }
            else
            {
                Trace.TraceError(LocalizationHelper.FindText("InvalidHelpFileType", helpName));
            }
        }
    }
}
