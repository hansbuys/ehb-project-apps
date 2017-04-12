using Common.Logging.Simple;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
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

            output.WriteLine($"Logging: {loggerEvent.RenderedMessage}");
        }
    }
}