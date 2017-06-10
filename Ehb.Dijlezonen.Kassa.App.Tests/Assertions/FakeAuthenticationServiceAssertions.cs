using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class FakeAuthenticationServiceAssertions : Assertions<FakeAuthenticationService, FakeAuthenticationServiceAssertions>
    {
        public FakeAuthenticationServiceAssertions(FakeAuthenticationService subject) : base(subject)
        {
        }

        internal AndConstraint<FakeAuthenticationServiceAssertions> BeLoggedIn()
        {
            IAuthenticationService subject = Subject;

            subject.LoggedInUser.Should().NotBeNull("we expected to be logged in");

            CheckedThat("we are logged in.");
            return And();
        }

        internal AndConstraint<FakeAuthenticationServiceAssertions> NotBeLoggedIn()
        {
            IAuthenticationService subject = Subject;

            subject.LoggedInUser.Should().BeNull("we expected to be logged out");

            CheckedThat("we are not logged in.");
            return And();
        }
    }
}