using Common.Logging.Simple;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    internal class TestLoggingAssertions : Assertions<TestLogging, TestLoggingAssertions>
    {
        public TestLoggingAssertions(TestLogging subject) : base(subject)
        {
        }

        public AndWhichConstraint<TestLoggingAssertions, CapturingLoggerEvent> HaveLoggedMessage(string message)
        {
            var loggingEvent = Subject.Events.Should().Contain(e => e.RenderedMessage == message).Which;

            CheckedThat($"'{message}' has been logged");

            return AndWhich(loggingEvent);
        }
    }
}
