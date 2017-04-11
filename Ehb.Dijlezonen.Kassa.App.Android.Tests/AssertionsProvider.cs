using log4net;

namespace Ehb.Dijlezonen.Kassa.App.Android.Tests
{
    internal static class AssertionsProvider
    {
        public static LogAssertions Should(this ILog logger)
        {
            return new LogAssertions(logger);
        }
    }
}
