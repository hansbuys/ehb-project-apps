using System;
using Common.Logging;

namespace Ehb.Dijlezonen.Kassa.Infrastructure
{
    public class Logging
    {
        private readonly Lazy<ILoggerFactoryAdapter> loggerFactory;
        private ILoggerFactoryAdapter LoggerFactory => loggerFactory.Value;
        
        public Logging(ILoggerFactoryAdapter loggerFactory = null)
        {
            this.loggerFactory = new Lazy<ILoggerFactoryAdapter>(() => loggerFactory ?? InitializeLoggerFactory());
        }

        protected virtual ILoggerFactoryAdapter InitializeLoggerFactory()
        {
            throw new Exception("Either implement this method in a derivate class or pass in the logger factory through the constructor.");
        }

        public ILog GetLoggerFor<T>()
        {
            return LoggerFactory.GetLogger(typeof(T));
        }
    }
}
