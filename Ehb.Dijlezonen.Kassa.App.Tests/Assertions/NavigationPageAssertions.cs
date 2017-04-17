using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class NavigationPageAssertions : Assertions<NavigationPage, NavigationPageAssertions>
    {
        public NavigationPageAssertions(NavigationPage subject) : base(subject)
        {
        }

        public void HaveARootPage()
        {
            Subject.CurrentPage.Should().NotBeNull();

            CheckedThat("we have a root page");
        }
    }
}