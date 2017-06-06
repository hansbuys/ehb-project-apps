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

        internal static LoginProviderAssertions Should(this FakeLoginProvider page)
        {
            return new LoginProviderAssertions(page);
        }

        internal static PageAssertions Should(this Page page)
        {
            return new PageAssertions(page);
        }

        internal static CommandAssertions Should(this Command page)
        {
            return new CommandAssertions(page);
        }
    }
}
