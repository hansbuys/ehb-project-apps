using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.iOS.Services
{
    // ReSharper disable once InconsistentNaming
    public class iOSLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        public iOSLoggerFactoryAdapter() : base(null)
        {
        }

        public iOSLoggerFactoryAdapter(NameValueCollection properties) : base(properties)
        {
        }

        public iOSLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel,
            string dateTimeFormat) : base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        {
        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime,
            bool showLogName,
            string dateTimeFormat)
        {
            return new iOSLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }
}