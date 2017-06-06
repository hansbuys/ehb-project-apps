using Autofac;
using Ehb.Dijlezonen.Kassa.App.Droid.Services;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Droid
{
    public class Bootstrapper : AppBootstrapperBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new AndroidLoggerFactoryAdapter()));
            builder.RegisterType<AndroidBackendConfiguration>().As<IBackendConfiguration>();
        }
    }
}