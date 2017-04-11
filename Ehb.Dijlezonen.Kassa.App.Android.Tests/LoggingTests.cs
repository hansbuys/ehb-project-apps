using log4net;
using Xunit;

namespace Ehb.Dijlezonen.Kassa.App.Android.Tests
{
    public class LoggingTests
    {
        private readonly ILog logger;

        public LoggingTests(ILog logger)
        {
            this.logger = logger;
        }

        [Fact]
        public void CanLogDebugMessage()
        {
            var message = "this is a test message";
            logger.Debug(message);

            logger.Should().HaveLoggedAsDebug(message);
        }
    }
}
