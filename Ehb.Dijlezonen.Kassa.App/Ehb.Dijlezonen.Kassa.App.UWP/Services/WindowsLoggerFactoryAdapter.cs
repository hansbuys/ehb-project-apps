using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace Ehb.Dijlezonen.Kassa.App.UWP.Services
{
    public class WindowsLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        public WindowsLoggerFactoryAdapter() : base(null)
        {
        }

        public WindowsLoggerFactoryAdapter(NameValueCollection properties) : base(properties)
        {
        }

        public WindowsLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat) : base(level, showDateTime, showLogName, showLevel, dateTimeFormat)
        {
        }

        protected override ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName,
            string dateTimeFormat)
        {
            return new WindowsLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat);
        }
    }
}