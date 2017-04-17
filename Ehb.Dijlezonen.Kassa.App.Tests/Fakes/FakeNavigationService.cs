using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Fakes
{
    public class FakeNavigationService : INavigationService
    {
        private readonly List<FakeNavigationEvent> navigationEvents = new List<FakeNavigationEvent>();
        private readonly Dictionary<Type, Type> registrations = new Dictionary<Type, Type>();
        public IEnumerable<FakeNavigationEvent> NavigationEvents => navigationEvents;
        public IReadOnlyDictionary<Type, Type> Registrations => registrations;

        void INavigationService.Register(Type view, Type viewModel)
        {
            registrations.Add(viewModel, view);
        }

        Task INavigationService.NavigateTo<T>()
        {
            navigationEvents.Add(new FakeNavigationEvent
            {
                DestinationType = typeof(T)
            });

            return Task.FromResult(0);
        }
    }

    public class FakeNavigationEvent
    {
        public Type DestinationType { get; set; }
    }
}