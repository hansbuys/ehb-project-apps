using Ehb.Dijlezonen.Kassa.App.Tests.Fakes;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal static class AssertionProvider
    {
        internal static NavigationServiceAssertions Should(this FakeNavigationService navigationService)
        {
            return new NavigationServiceAssertions(navigationService);
        }

        internal static PageAssertions Should(this Page page)
        {
            return new PageAssertions(page);
        }

        internal static NavigationPageAssertions Should(this NavigationPage page)
        {
            return new NavigationPageAssertions(page);
        }
    }
}
