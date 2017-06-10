using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.Droid.Services
{
    public class AndroidLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        public AndroidLoggerFactoryAdapter() : base(null)
        {
        }

        public AndroidLoggerFactoryAdapter(NameValueCollection properties) : base(properties)
        {
        }

        public AndroidLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel,
            string dateTimeFormat) :
            base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        {
        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime,
            bool showLogName,
            string dateTimeFormat)
        {
            return new AndroidLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }
}