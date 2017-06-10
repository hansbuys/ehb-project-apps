using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public abstract class AppBootstrapperBase : BootstrapperBase
    {
        protected override void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().SingleInstance();

            //this is due to the events
            builder.RegisterType<Navigation>().SingleInstance();

            builder.RegisterInstance(Duty.AllDuties);
        }
    }
}