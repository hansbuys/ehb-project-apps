using System;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public abstract class TestBase
    {
        private readonly Lazy<TestLogging> logging;
        private readonly ITestOutputHelper output;

        protected TestBase(ITestOutputHelper output)
        {
            this.output = output;

            logging = new Lazy<TestLogging>(() => new TestLogging(this.output));

            Assertions.WriteTestOutput = this.output.WriteLine;
        }

        protected TestLogging Logging => logging.Value;
    }
}