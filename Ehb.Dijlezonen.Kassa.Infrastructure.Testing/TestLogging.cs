using System.Collections.Generic;
using Common.Logging;
using Common.Logging.Simple;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public class TestLogging : Logging
    {
        private readonly TestLoggerFactoryAdapter loggerFactoryAdapter;

        public TestLogging(ITestOutputHelper output)
        {
            Output = output;
            loggerFactoryAdapter = new TestLoggerFactoryAdapter(Output);
        }
        
        private ITestOutputHelper Output { get; }
        public IEnumerable<CapturingLoggerEvent> Events => loggerFactoryAdapter.LoggerEvents;

        protected override ILoggerFactoryAdapter InitializeLoggerFactory()
        {
            return loggerFactoryAdapter;
        }
    }
}