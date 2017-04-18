using Autofac;
using Ehb.Dijlezonen.Kassa.App.iOS.Services;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.iOS
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new iOSLoggerFactoryAdapter()));
        }
    }
}