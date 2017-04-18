using Ehb.Dijlezonen.Kassa.App.Testing;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal static class AssertionProvider
    {
        internal static NavigationAssertions Should(this FakeNavigation navigation)
        {
            return new NavigationAssertions(navigation);
        }

        internal static AccountStoreAssertions Should(this FakeAccountStore page)
        {
            return new AccountStoreAssertions(page);
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
