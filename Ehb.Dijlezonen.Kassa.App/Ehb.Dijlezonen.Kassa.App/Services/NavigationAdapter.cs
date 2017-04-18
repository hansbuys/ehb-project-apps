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

        private ILog Log => log.Value;
        private readonly Lazy<ILog> log = new Lazy<ILog>(() => {
            var logging = IoC.Container.Resolve<Logging>();

            return logging.GetLoggerFor<NavigationAdapter>();
        });
        private ViewFactory ViewFactory => viewFactory.Value;
        private readonly Lazy<ViewFactory> viewFactory = new Lazy<ViewFactory>(() => 
            IoC.Container.Resolve<ViewFactory>());

        public NavigationAdapter(INavigation navigation)
        {
            this.navigation = navigation;
        }

        public Task NavigateTo<TViewModel>()
        {
            Page view = ViewFactory.ResolveViewFor<TViewModel>();

            Log.Info($"Navigating to {view.GetType().Name}");

            return navigation.PushAsync(view);
        }

        public Task CloseModal()
        {
            return navigation.PopModalAsync();
        }

        public Task NavigateToModal<TViewModel>()
        {
            Page view = ViewFactory.ResolveViewFor<TViewModel>();

            Log.Info($"Navigating modally to {view.GetType().Name}");

            return navigation.PushModalAsync(view);
        }
    }
}