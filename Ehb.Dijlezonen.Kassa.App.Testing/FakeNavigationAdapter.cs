using System;
using System.Collections.Concurrent;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using System.Threading.Tasks;
using System.Linq;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeNavigationAdapter : INavigationAdapter
    {
        public ConcurrentDictionary<Type, object> ModalStack { get; } = new ConcurrentDictionary<Type, object>();
        public ConcurrentDictionary<Type, object> NavigationStack { get; } = new ConcurrentDictionary<Type, object>();

        private readonly ILog log;

        public FakeNavigationAdapter(Logging logging)
        {
            this.log = logging.GetLoggerFor<FakeNavigationAdapter>();
        }

        internal void SetResolver(IContainer container)
        {
            Container = container;
        }

        public IContainer Container { get; private set; }

        Task INavigationAdapter.NavigateTo<TViewModel>()
        {
            log.Debug($"Navigating to {typeof(TViewModel).Name}");

            NavigationStack.AddOrUpdate(typeof(TViewModel), t => Container.Resolve<TViewModel>(),
                (t, k) => throw new Exception("You already have this view on the stack"));
            return Task.FromResult(0);
        }

        Task INavigationAdapter.NavigateToModal<TViewModel>()
        {
            log.Debug($"Navigating modally to {typeof(TViewModel).Name}"); 

            ModalStack.AddOrUpdate(typeof(TViewModel), t => Container.Resolve<TViewModel>(), 
                (t, k) => throw new Exception("You already have this view on the modal stack"));
            return Task.FromResult(0);
        }

        Task INavigationAdapter.CloseModal()
        {
            ModalStack.TryRemove(ModalStack.Last().Key, out object vm);

            log.Debug($"Closing modal view {vm.GetType().Name}");
            return Task.FromResult(0);
        }
    }
}