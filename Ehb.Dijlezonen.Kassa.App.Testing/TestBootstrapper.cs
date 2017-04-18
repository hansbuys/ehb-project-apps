using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class TestBootstrapper : BootstrapperBase
    {
        private TestLogging logging;

        public TestBootstrapper(TestLogging logging)
        {
            this.logging = logging;
        }

        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(logging).As<Logging>();
        }
    }
}