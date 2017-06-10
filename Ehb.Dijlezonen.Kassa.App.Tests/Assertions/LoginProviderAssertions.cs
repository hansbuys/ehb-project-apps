using System;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class LoginProviderAssertions : Assertions<FakeLoginProvider, LoginProviderAssertions>
    {
        public LoginProviderAssertions(FakeLoginProvider subject) : base(subject)
        {
        }

        internal AndConstraint<LoginProviderAssertions> BeLoggedIn()
        {
            ILoginProvider loginProvider = Subject;
            var isLoggedIn = loginProvider.IsLoggedIn().Result;
            isLoggedIn.Should().BeTrue("we expected to be logged in");

            CheckedThat("we are logged in.");
            return And();
        }

        internal AndConstraint<LoginProviderAssertions> NotBeLoggedIn()
        {
            ILoginProvider loginProvider = Subject;
            var isLoggedIn = loginProvider.IsLoggedIn().Result;
            isLoggedIn.Should().BeFalse("we expected to be logged out");

            CheckedThat("we are not logged in.");
            return And();
        }
    }
}