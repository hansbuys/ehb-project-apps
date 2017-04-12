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
        private readonly ILog logger;
        private readonly TestLogging logging;

        public LoggingTests(ITestOutputHelper output) : base(output)
        {
            logging = new TestLogging(output);
            logger = logging.GetLoggerFor<LoggingTests>();
        }

        [Fact]
        public void CanLogDebugMessage()
        {
            const string message = "this is a test message";

            logger.Debug(message);

            var loggingEvent = logging.Should().HaveLoggedMessage(message).Which;
            loggingEvent.Level.Should().Be(LogLevel.Debug);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("test", "something else")]
        public void HaveLoggedAssertionCanFail(string message, string expectedMessage)
        {
            logger.Debug(message);

            Action assert = () => logging.Should().HaveLoggedMessage(expectedMessage);

            if (message == expectedMessage)
                assert();
            else
                Assert.Throws<XunitException>(assert);
        }
    }
}
