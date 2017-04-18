using System;
using Common.Logging;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.UWP.Services
{
    public class WindowsLogger : AbstractSimpleLogger
    {
        public WindowsLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat) : base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            var format = $"{Name} - {level} : {message}";

            if (exception != null)
                format += exception.ToString();

            System.Diagnostics.Debug.WriteLine(format);
        }
    }
}