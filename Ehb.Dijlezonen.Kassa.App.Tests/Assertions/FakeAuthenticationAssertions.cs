using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class FakeAuthenticationAssertions : Assertions<FakeAuthentication, FakeAuthenticationAssertions>
    {
        public FakeAuthenticationAssertions(FakeAuthentication subject) : base(subject)
        {
        }

        internal AndConstraint<FakeAuthenticationAssertions> BeLoggedIn()
        {
            IAuthentication subject = Subject;

            subject.LoggedInUser.Should().NotBeNull("we expected to be logged in");

            CheckedThat("we are logged in.");
            return And();
        }

        internal AndConstraint<FakeAuthenticationAssertions> NotBeLoggedIn()
        {
            IAuthentication subject = Subject;

            subject.LoggedInUser.Should().BeNull("we expected to be logged out");

            CheckedThat("we are not logged in.");
            return And();
        }
    }
}