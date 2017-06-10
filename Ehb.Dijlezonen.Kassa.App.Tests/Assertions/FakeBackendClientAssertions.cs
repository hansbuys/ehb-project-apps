using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class FakeBackendClientAssertions : Assertions<FakeBackendClient, FakeBackendClientAssertions>
    {
        public FakeBackendClientAssertions(FakeBackendClient subject) : base(subject)
        {
        }

        internal AndConstraint<FakeBackendClientAssertions> BeLoggedIn()
        {
            IBackendClient subject = Subject;

            subject.LoggedInUser.Should().NotBeNull("we expected to be logged in");

            CheckedThat("we are logged in.");
            return And();
        }

        internal AndConstraint<FakeBackendClientAssertions> NotBeLoggedIn()
        {
            IBackendClient subject = Subject;

            subject.LoggedInUser.Should().BeNull("we expected to be logged out");

            CheckedThat("we are not logged in.");
            return And();
        }
    }
}