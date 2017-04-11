namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    internal static class AssertionsProvider
    {
        public static TestLoggingAssertions Should(this TestLogging logger)
        {
            return new TestLoggingAssertions(logger);
        }
    }
}
