using System.Threading.Tasks;
using Xamarin.Forms;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Common.Logging;
using System;
using Autofac;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class NavigationAdapter : INavigationAdapter
    {
        private readonly INavigation navigation;

        private ILog log;
        private ViewFactory viewFactory;

        public NavigationAdapter(INavigation navigation, Logging logging, ViewFactory viewFactory)
        {
            this.navigation = navigation;
            log = logging.GetLoggerFor<NavigationAdapter>();
            this.viewFactory = viewFactory;
        }

        public Task NavigateTo<TViewModel>()
        {
            Page view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating to {view.GetType().Name}");

            return navigation.PushAsync(view);
        }

        public Task CloseModal()
        {
            return navigation.PopModalAsync();
        }

        public Task NavigateToModal<TViewModel>()
        {
            Page view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating modally to {view.GetType().Name}");

            return navigation.PushModalAsync(view);
        }
    }
}