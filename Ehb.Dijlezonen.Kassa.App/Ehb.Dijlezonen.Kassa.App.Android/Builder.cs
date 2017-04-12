using Autofac;
using Ehb.Dijlezonen.Kassa.App.Droid.Services;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Droid
{
    public class Builder : AppBuilderBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new AndroidLoggerFactoryAdapter()));
        }

        protected override void RegisterComponents(ContainerBuilder builder)
        {
        }
    }
}