using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;
using System.Linq;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class NavigatorAssertions : Assertions<FakeNavigationAdapter, NavigatorAssertions>
    {
        public NavigatorAssertions(FakeNavigationAdapter subject) : base(subject)
        {
        }

        internal AndWhichConstraint<NavigatorAssertions, TViewModel> BeDisplaying<TViewModel>(bool modalOnly = false)
            where TViewModel : class
        {
            var vm = Subject.ModalStack.FirstOrDefault();

            if (vm == null && !modalOnly)
                vm = Subject.NavigationStack.FirstOrDefault();

            vm.Should().NotBeNull("we expected to be displaying something");

            vm.Should().BeOfType<TViewModel>($"we expected to be displaying the viewmodel of type {typeof(TViewModel).Name}");

            CheckedThat($"we are displaying a viewmodel of type '{typeof(TViewModel).Name}'");

            return AndWhich((TViewModel)vm);
        }

        internal AndConstraint<NavigatorAssertions> NotBeDisplaying<TViewModel>()
            where TViewModel : class
        {
            var vm = Subject.ModalStack.FirstOrDefault() ?? Subject.NavigationStack.FirstOrDefault();
            
            vm?.Should().NotBeOfType<TViewModel>($"we didn't expected to be displaying the viewmodel of type {typeof(TViewModel).Name}");

            CheckedThat($"we are not displaying a viewmodel of type '{typeof(TViewModel).Name}'");

            return And();
        }
    }
}