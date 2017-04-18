using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class PageAssertions : Assertions<Page, PageAssertions>
    {
        public PageAssertions(Page subject) : base(subject)
        {
        }

        public AndWhichConstraint<PageAssertions, NavigationPage> BeANavigationPage()
        {
            var navigationPage = Subject.Should().BeANavigationPage().Which;

            CheckedThat($"{Subject.GetType().Name} is a navigation page");

            return AndWhich(navigationPage);
        }

        public void HaveRootPage()
        {
            throw new System.NotImplementedException();
        }
    }
}