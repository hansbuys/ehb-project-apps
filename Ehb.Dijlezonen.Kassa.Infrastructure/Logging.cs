using System;
using System.Diagnostics;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Ehb.Dijlezonen.Kassa.Infrastructure
{
    public class Logging : IDisposable
    {
        private readonly WrapperMap wrapperMap;
        private readonly Lazy<Hierarchy> hierarchy;

        private Hierarchy Hierarchy => hierarchy.Value;

        /// <summary>
        /// add this to each DI container with <code>LogManager.GetRepository()</code> as constructor value
        /// </summary>
        /// <param name="hierarchy"></param>
        public Logging(Hierarchy hierarchy = null)
        {
            wrapperMap = new WrapperMap(WrapperCreationHandler);
            Debug.WriteLine("Constructing Logging...");
            this.hierarchy = new Lazy<Hierarchy>(() => hierarchy ?? InitalizeHierarchy());
        }

        private static ILoggerWrapper WrapperCreationHandler(ILogger logger)
        {
            return new LogImpl(logger);
        }

        protected virtual Hierarchy InitalizeHierarchy()
        {
            throw new NotImplementedException();
        }

        public ILog GetLoggerFor<T>()
        {
            return (ILog)wrapperMap.GetWrapper(Hierarchy.GetLogger(typeof(T).Name));
        }

        #region IDisposable Support

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    if (hierarchy.IsValueCreated)
                        hierarchy.Value.Shutdown();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
