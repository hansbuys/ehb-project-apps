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
            var vm = Subject.NavigationStack.Last();
            vm.Should().BeOfType<TViewModel>();

            CheckedThat($"we have navigated to view model '{typeof(TViewModel).Name}'");

            return AndWhich((TViewModel)vm);
        }

        internal AndWhichConstraint<NavigatorAssertions, TViewModel> HaveNavigatedToModal<TViewModel>()
            where TViewModel : class
        {
            var vm = Subject.ModalStack.Last();
            vm.Should().BeOfType<TViewModel>();
            
            CheckedThat($"we have modally navigated to view model '{typeof(TViewModel).Name}'");

            return AndWhich((TViewModel)vm);
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