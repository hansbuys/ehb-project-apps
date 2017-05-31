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
        public ConcurrentStack<object> ModalStack { get; } = new ConcurrentStack<object>();
        public ConcurrentStack<object> NavigationStack { get; } = new ConcurrentStack<object>();

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

        Task<TViewModel> INavigationAdapter.NavigateTo<TViewModel>()
        {
            log.Debug($"Navigating to {typeof(TViewModel).Name}");

            var viewModel = Container.Resolve<TViewModel>();
            NavigationStack.Push(viewModel);

            return Task.FromResult(viewModel);
        }

        Task<TViewModel> INavigationAdapter.NavigateToModal<TViewModel>()
        {
            log.Debug($"Navigating modally to {typeof(TViewModel).Name}");

            var viewModel = Container.Resolve<TViewModel>();
            ModalStack.Push(viewModel);

            return Task.FromResult(viewModel);
        }

        Task INavigationAdapter.CloseModal()
        {
            object vm;
            ModalStack.TryPop(out vm);

            log.Debug($"Closing modal view {vm.GetType().Name}");
            return Task.FromResult(0);
        }
    }
}