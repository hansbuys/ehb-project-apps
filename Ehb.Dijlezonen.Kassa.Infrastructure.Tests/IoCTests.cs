using System;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    public class IoCTests : IoCBasedTest<IoCTests.X>
    {
        private class Bootstrapper : BootstrapperBase
        {
            private readonly TestLogging logging;

            public Bootstrapper(TestLogging logging)
            {
                this.logging = logging;
            }

            protected override void RegisterPorts(ContainerBuilder builder)
            {
                builder.RegisterInstance(logging).As<Logging>();
            }
            
            protected override void RegisterComponents(ContainerBuilder builder)
            {
            }
        }

        public class X
        {
            private readonly Func<string, Y> getY;
            private readonly ILog logger;

            public X(Func<string, Y> getY, Logging logging)
            {
                this.getY = getY;
                this.logger = logging.GetLoggerFor<X>();
            }

            public string Echo(string value)
            {
                var echo = getY(value).Arg;
                logger.Debug($"Echoing: {echo}");
                return echo;
            }
        }

        public class Y
        {
            public Y(string arg)
            {
                Arg = arg;
            }

            public string Arg { get; }
        }

        public IoCTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanConstructUsingFactory()
        {
            const string expected = "test";

            var echo = (await GetSut()).Echo(expected);

            echo.Should().Be(expected);
        }

        protected override BootstrapperBase GetBootstrapper()
        {
            return new Bootstrapper(Logging);
        }
    }
}
