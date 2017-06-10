using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xamarin.Forms;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class CommandAssertions : Assertions<Command, CommandAssertions>
    {
        public CommandAssertions(Command subject) : base(subject)
        {
        }

        internal AndConstraint<CommandAssertions> BeDisabled()
        {
            Subject.CanExecute(null).Should().BeFalse();

            CheckedThat("command is not executable at this time.");

            return And();
        }

        internal AndConstraint<CommandAssertions> BeEnabled()
        {
            Subject.CanExecute(null).Should().BeTrue();

            CheckedThat("command is executable at this time.");

            return And();
        }
    }
}