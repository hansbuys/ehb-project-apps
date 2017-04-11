using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Tests
{
    public class TestAppender : AppenderSkeleton
    {
        private readonly List<LoggingEvent> messages;
        private readonly List<string> renderedMessages;
        private readonly ITestOutputHelper output;

        public TestAppender(ITestOutputHelper output, List<LoggingEvent> messages, ILayout layout, List<string> renderedMessages)
        {
            this.output = output;
            this.messages = messages;
            this.renderedMessages = renderedMessages;

            Layout = layout;
        }

        protected override bool RequiresLayout => true;

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                var rendered = RenderLoggingEvent(loggingEvent);
                WriteEventToTestOutput(rendered);
                messages.Add(loggingEvent);
                renderedMessages.Add(rendered);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc);
            }
        }

        private void WriteEventToTestOutput(string loggingEvent)
        {
            output.WriteLine(loggingEvent);
        }
    }
}