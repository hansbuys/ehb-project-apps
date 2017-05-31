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

        public async Task<TViewModel> NavigateTo<TViewModel>()
        {
            var view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating to {view.GetType().Name}");

            await navigation.PushAsync(view);

            return (TViewModel)view.BindingContext;
        }

        public Task CloseModal()
        {
            return navigation.PopModalAsync();
        }

        public async Task<TViewModel> NavigateToModal<TViewModel>()
        {
            var view = viewFactory.ResolveViewFor<TViewModel>();

            log.Info($"Navigating modally to {view.GetType().Name}");

            await navigation.PushModalAsync(view);

            return (TViewModel)view.BindingContext;
        }
    }
}