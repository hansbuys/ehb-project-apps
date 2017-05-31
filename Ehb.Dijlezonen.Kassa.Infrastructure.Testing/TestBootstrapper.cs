using Autofac;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public class TestBootstrapper : Bootstrapper
    {
        private readonly TestLogging logging;

        public TestBootstrapper(TestLogging logging)
        {
            this.logging = logging;
        }

        protected override void RegisterPorts(ContainerBuilder builder)
        {
        }

        protected override void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterInstance(logging).As<Logging>();
        }
    }
}