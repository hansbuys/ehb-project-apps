using Ehb.Dijlezonen.Kassa.App.Tests.Fakes;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using FluentAssertions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Assertions
{
    internal class NavigationServiceAssertions : Assertions<FakeNavigationService, NavigationServiceAssertions>
    {
        public NavigationServiceAssertions(FakeNavigationService subject) : base(subject)
        {
        }

        public AndWhichConstraint<NavigationServiceAssertions, FakeNavigationEvent> HaveNavigatedTo<T>()
        {
            var navigationEvent =
                Subject.NavigationEvents.Should().ContainSingle(e => e.DestinationType == typeof(T)).Which;

            CheckedThat($"we have navigated to view model '{typeof(T).Name}'");

            return AndWhich(navigationEvent);
        }

        public AndConstraint<NavigationServiceAssertions> HaveRegistered<TView, TViewModel>()
        {
            Subject.Registrations.Should().ContainSingle(e => e.Key == typeof(TViewModel) && e.Value == typeof(TView));

            CheckedThat($"view '{typeof(TView).Name}' has been registered with view model '{typeof(TViewModel).Name}'");

            return And();
        }
    }
}