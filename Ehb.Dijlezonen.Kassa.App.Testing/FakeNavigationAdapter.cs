using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeNavigationAdapter : INavigationAdapter
    {
        private readonly IComponentContext container;

        private readonly ILog log;

        public FakeNavigationAdapter(Logging logging, IComponentContext container)
        {
            this.container = container;
            log = logging.GetLoggerFor<FakeNavigationAdapter>();
        }

        public ConcurrentStack<object> ModalStack { get; } = new ConcurrentStack<object>();
        public ConcurrentStack<object> NavigationStack { get; } = new ConcurrentStack<object>();

        Task<TViewModel> INavigationAdapter.NavigateTo<TViewModel>()
        {
            log.Debug($"Navigating to {typeof(TViewModel).Name}");

            var viewModel = container.Resolve<TViewModel>();
            NavigationStack.Push(viewModel);

            return Task.FromResult(viewModel);
        }

        Task<TViewModel> INavigationAdapter.NavigateToModal<TViewModel>()
        {
            log.Debug($"Navigating modally to {typeof(TViewModel).Name}");

            var viewModel = container.Resolve<TViewModel>();
            ModalStack.Push(viewModel);

            return Task.FromResult(viewModel);
        }

        Task INavigationAdapter.CloseModal()
        {
            object vm;
            ModalStack.TryPop(out vm);

            var disposable = vm as IDisposable;
            disposable?.Dispose();

            log.Debug($"Closing modal view {vm.GetType().Name}");
            return Task.FromResult(0);
        }
    }
}