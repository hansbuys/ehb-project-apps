using System.Linq;
using Common.Logging.Simple;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    internal class TestLoggerFactoryAdapter : CapturingLoggerFactoryAdapter
    {
        private readonly ITestOutputHelper output;

        public TestLoggerFactoryAdapter(ITestOutputHelper output)
        {
            this.output = output;
        }

        public override void AddEvent(CapturingLoggerEvent loggerEvent)
        {
            base.AddEvent(loggerEvent);

            var shortSourceName = loggerEvent.Source.Name.Split('.').Last();

            output.WriteLine($"{shortSourceName} : {loggerEvent.RenderedMessage}");
        }
    }
}