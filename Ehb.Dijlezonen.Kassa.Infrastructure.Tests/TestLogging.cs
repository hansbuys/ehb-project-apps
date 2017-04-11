using System.Collections.Generic;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    internal class TestLogging : Logging
    {
        private readonly List<string> logOutput = new List<string>();

        public TestLogging(ITestOutputHelper output, ILayout layout)
        {
            Output = output;
            Layout = layout;
        }

        public List<LoggingEvent> Events { get; } = new List<LoggingEvent>();

        private ITestOutputHelper Output { get; }

        private ILayout Layout { get; }

        protected override Hierarchy InitalizeHierarchy()
        {
            var hierarchy = new Hierarchy();
            var appender = new TestAppender(Output, Events, Layout, logOutput);
            hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;

            return hierarchy;
        }
    }
}