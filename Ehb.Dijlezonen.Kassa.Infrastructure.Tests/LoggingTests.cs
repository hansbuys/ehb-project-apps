using System;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    public class LoggingTests : TestBase
    {
        private readonly Lazy<ILog> logger;
        private ILog Logger => logger.Value;

        public LoggingTests(ITestOutputHelper output) : base(output)
        {
            logger = new Lazy<ILog>(() => Logging.GetLoggerFor<LoggingTests>());
        }

        [Fact]
        public void CanLogDebugMessage()
        {
            const string message = "this is a test message";

            Logger.Debug(message);

            var loggingEvent = Logging.Should().HaveLoggedMessage(message).Which;
            loggingEvent.Level.Should().Be(LogLevel.Debug);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test", "something else")]
        public void HaveLoggedAssertionCanFail(string message, string expectedMessage)
        {
            Logger.Debug(message);

            Action assert = () => Logging.Should().HaveLoggedMessage(expectedMessage);

            if (message == expectedMessage)
                assert();
            else
                Assert.Throws<XunitException>(assert);
        }
    }
}
