using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class XunitLoggingProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper output;

        public XunitLoggingProvider(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new OutputLogger(output);
        }
    }
}