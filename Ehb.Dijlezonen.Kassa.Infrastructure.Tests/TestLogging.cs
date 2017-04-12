using System.Collections.Generic;
using Common.Logging;
using Common.Logging.Simple;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    internal class TestLogging : Logging
    {
        private CapturingLoggerFactoryAdapter loggerFactory;

        public TestLogging(ITestOutputHelper output)
        {
            Output = output;
        }
        
        private ITestOutputHelper Output { get; }
        public IEnumerable<CapturingLoggerEvent> Events => loggerFactory.LoggerEvents;

        protected override ILoggerFactoryAdapter InitializeLoggerFactory()
        {
            loggerFactory = new TestLoggerFactoryAdapter(Output);

            return loggerFactory;
        }
    }
}