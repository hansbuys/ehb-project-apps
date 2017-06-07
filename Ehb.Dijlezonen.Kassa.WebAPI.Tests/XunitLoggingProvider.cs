using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests
{
    public class XunitLoggingProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper output;
        private readonly IConfigurationSection config;

        public XunitLoggingProvider(ITestOutputHelper output, IConfigurationSection config)
        {
            this.output = output;
            this.config = config;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new OutputLogger(categoryName, output, GetFilter(categoryName, new ConfigurationConsoleLoggerSettings(config)));
        }
        
        private Func<string, LogLevel, bool> GetFilter(string name, IConsoleLoggerSettings settings)
        {
            if (settings != null)
            {
                foreach (var keyPrefix in GetKeyPrefixes(name))
                {
                    LogLevel level;
                    if (settings.TryGetSwitch(keyPrefix, out level))
                    {
                        return (n, l) => l >= level;
                    }
                }
            }
            return (n, l) => false;
        }

        private IEnumerable<string> GetKeyPrefixes(string name)
        {
            int lastIndexOfDot;
            for (; !string.IsNullOrEmpty(name); name = name.Substring(0, lastIndexOfDot))
            {
                yield return name;
                lastIndexOfDot = name.LastIndexOf('.');
                if (lastIndexOfDot == -1)
                {
                    yield return "Default";
                    break;
                }
            }
        }
    }
}