using System;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class AccountStoreAssertions : Assertions<FakeAccountStore, AccountStoreAssertions>
    {
        public AccountStoreAssertions(FakeAccountStore subject) : base(subject)
        {
        }

        internal AndConstraint<AccountStoreAssertions> BeLoggedIn()
        {
            IAccountStore accountStore = Subject;
            var isLoggedIn = accountStore.IsLoggedIn().Result;
            isLoggedIn.Should().BeTrue("we expected to be logged in");

            CheckedThat("we are logged in.");
            return And();
        }

        internal AndConstraint<AccountStoreAssertions> NotBeLoggedIn()
        {
            IAccountStore accountStore = Subject;
            var isLoggedIn = accountStore.IsLoggedIn().Result;
            isLoggedIn.Should().BeFalse("we expected to be logged out");

            CheckedThat("we are not logged in.");
            return And();
        }
    }
}