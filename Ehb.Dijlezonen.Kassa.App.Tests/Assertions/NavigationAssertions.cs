using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using System.Linq;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class NavigatorAssertions : Assertions<FakeNavigationAdapter, NavigatorAssertions>
    {
        public NavigatorAssertions(FakeNavigationAdapter subject) : base(subject)
        {
        }

        public AndWhichConstraint<NavigatorAssertions, TViewModel> HaveNavigatedTo<TViewModel>()
            where TViewModel : class
        {
            var viewModel = Subject.NavigationStack.Last().Should().BeOfType<TViewModel>().Which;

            CheckedThat($"we have navigated to view model '{viewModel.GetType().Name}'");

            return AndWhich(viewModel);
        }

        internal AndWhichConstraint<NavigatorAssertions, TViewModel> HaveNavigatedToModal<TViewModel>()
            where TViewModel : class
        {
            var viewModel = Subject.ModalStack.Last().Should().BeOfType<TViewModel>().Which;

            CheckedThat($"we have modally navigated to view model '{viewModel.GetType().Name}'");

            return AndWhich(viewModel);
        }

        internal AndConstraint<NavigatorAssertions> NotHaveModal<TViewModel>()
        {
            Subject.ModalStack.Should().NotContain(p => 
                p.GetType() == typeof(TViewModel));

            CheckedThat($"we don't have a modal view open for {typeof(TViewModel).Name}");

            return And();
        }
    }
}