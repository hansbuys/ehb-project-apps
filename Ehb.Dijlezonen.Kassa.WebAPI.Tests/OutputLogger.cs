using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class OutputLogger : ILogger
    {
        private readonly string name;
        private readonly ITestOutputHelper output;
        private Func<string, LogLevel, bool> filter;

        public OutputLogger(string name, ITestOutputHelper output, Func<string, LogLevel, bool> filter)
        {
            this.name = name;
            this.output = output;
            this.filter = filter;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            output.WriteLine(formatter(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return filter(name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}