using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using System.Linq;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class NavigationAssertions : Assertions<FakeNavigation, NavigationAssertions>
    {
        public NavigationAssertions(FakeNavigation subject) : base(subject)
        {
        }

        public AndWhichConstraint<NavigationAssertions, TViewModel> HaveNavigatedTo<TView, TViewModel>()
            where TView : Page
            where TViewModel : class
        {
            INavigation navigation = Subject;

            var viewModel = navigation.NavigationStack.Last().Should().BeOfType<TView>().Which;

            CheckedThat($"we have navigated to view model '{viewModel.GetType().Name}'");

            return AndWhich((TViewModel)viewModel.BindingContext);
        }

        internal AndWhichConstraint<NavigationAssertions, TViewModel> HaveNavigatedToModal<TView, TViewModel>()
            where TView : Page
            where TViewModel : class
        {
            INavigation navigation = Subject;

            var viewModel = navigation.ModalStack.Last().Should().BeOfType<TView>().Which;

            CheckedThat($"we have modally navigated to view model '{viewModel.GetType().Name}'");

            return AndWhich((TViewModel)viewModel.BindingContext);
        }
    }
}