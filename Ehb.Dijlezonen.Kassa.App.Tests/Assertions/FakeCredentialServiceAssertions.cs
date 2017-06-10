using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class FakeCredentialServiceAssertions : Assertions<FakeCredentialService, FakeCredentialServiceAssertions>
    {
        internal FakeCredentialServiceAssertions(FakeCredentialService subject) : base(subject)
        {
        }

        public void HaveRegistered(string firstname, string lastname)
        {
            Subject.Registrations.Should().ContainSingle(x =>
                x.Firstname == firstname && 
                x.Lastname == lastname);
        }
    }
}
