using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public abstract class TestBase
    {
        protected ITestOutputHelper Output { get; }

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;

            Assertions.WriteTestOutput = Output.WriteLine;
        }
    }
}
