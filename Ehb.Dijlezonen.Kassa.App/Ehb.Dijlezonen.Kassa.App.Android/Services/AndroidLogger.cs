using System;
using Android.Util;
using Common.Logging;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.Droid.Services
{
    public class AndroidLogger : AbstractSimpleLogger
    {
        public AndroidLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName,
            string dateTimeFormat) : base(logName, logLevel, showlevel, showDateTime, showLogName, dateTimeFormat)
        {
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            var tag = Name;
            var format = message.ToString();

            var logPriority = ToLogPriority(level);

            if (logPriority.HasValue)
                if (exception == null)
                    Log.WriteLine(logPriority.Value, tag, format);
                else
                    Log.Wtf(tag, format, exception);
        }

        private static LogPriority? ToLogPriority(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return LogPriority.Verbose;
                case LogLevel.Trace:
                    return LogPriority.Verbose;
                case LogLevel.Debug:
                    return LogPriority.Debug;
                case LogLevel.Info:
                    return LogPriority.Info;
                case LogLevel.Warn:
                    return LogPriority.Warn;
                case LogLevel.Error:
                    return LogPriority.Error;
                case LogLevel.Fatal:
                    return LogPriority.Error;
                default:
                    return null;
            }
        }
    }
}