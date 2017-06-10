using Ehb.Dijlezonen.Kassa.App.Testing;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal static class AssertionProvider
    {
        internal static NavigatorAssertions Should(this FakeNavigationAdapter navigation)
        {
            return new NavigatorAssertions(navigation);
        }

        internal static FakeAuthenticationAssertions Should(this FakeAuthentication authentication)
        {
            return new FakeAuthenticationAssertions(authentication);
        }

        internal static FakeCredentialServiceAssertions Should(this FakeCredentialService credentials)
        {
            return new FakeCredentialServiceAssertions(credentials);
        }

        internal static CommandAssertions Should(this Command page)
        {
            return new CommandAssertions(page);
        }
    }
}
