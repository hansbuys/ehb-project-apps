using System;
using Common.Logging;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.iOS.Services
{
    // ReSharper disable once InconsistentNaming
    public class iOSLogger : AbstractSimpleLogger
    {
        public iOSLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat) : base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            //TODO: implement logging output for iOS
        }
    }
}