using System;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public abstract class TestBase
    {
        private readonly Lazy<TestLogging> logging;

        protected TestBase(ITestOutputHelper output)
        {
            logging = new Lazy<TestLogging>(() => new TestLogging(output));

            Assertions.WriteTestOutput = output.WriteLine;
        }

        protected TestLogging Logging => logging.Value;
    }
}