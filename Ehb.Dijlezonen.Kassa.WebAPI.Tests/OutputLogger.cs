using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class OutputLogger : ILogger
    {
        private readonly ITestOutputHelper output;

        public OutputLogger(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            output.WriteLine(formatter(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}