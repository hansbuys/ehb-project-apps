using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Common.Logging;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class NavigationAdapter : INavigationAdapter
    {
        private readonly INavigation navigation;

        private readonly ILog log;
        private readonly ViewFactory viewFactory;

        public NavigationAdapter(INavigation navigation, Logging logging, ViewFactory viewFactory)
        {
            this.navigation = navigation;
            log = logging.GetLoggerFor<NavigationAdapter>();
            this.viewFactory = viewFactory;
        }

        async Task<TViewModel> INavigationAdapter.NavigateTo<TViewModel>()
        {
            var view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating to {view.GetType().Name}");

            await navigation.PushAsync(view);

            return (TViewModel)view.BindingContext;
        }

        async Task INavigationAdapter.CloseModal()
        {
            var modal = await navigation.PopModalAsync();

            DisposeViewModel(modal, true);
        }

        async Task INavigationAdapter.Close()
        {
            var modal = await navigation.PopAsync();

            DisposeViewModel(modal, false);
        }

        private void DisposeViewModel(BindableObject page, bool isModal)
        {
            if (page == null) return;

            var modal = isModal ? "modal" : "";
            log.Debug($"Closing {modal} view {page.GetType().Name}");

            var vm = page.BindingContext as IDisposable;
            if (vm != null)
            {
                log.Debug($"Disposing viewmodel {vm.GetType().Name}");
                vm.Dispose();
            }
        }

        async Task<TViewModel> INavigationAdapter.NavigateToModal<TViewModel>()
        {
            var view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating modally to {view.GetType().Name}");
            
            await navigation.PushModalAsync(view);

            return (TViewModel)view.BindingContext;
        }
    }
}