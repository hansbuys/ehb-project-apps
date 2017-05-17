using System.Threading.Tasks;
using System.Windows.Input;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel
    {
        private readonly IAccountStore auth;
        private readonly ILog log;
        private readonly INavigationAdapter navigation;

        public MainPageViewModel(INavigationAdapter navigation, IAccountStore auth, Logging logging)
        {
            this.navigation = navigation;
            this.auth = auth;
            log = logging.GetLoggerFor<MainPageViewModel>();

            Initialize().Wait();
        }

        public string Title => "De Dijlezonen Kassa";

        public string LogoutCommandText => "Uitloggen";
        public ICommand LogoutCommand => new Command(async () => await Logout().ConfigureAwait(false));

        private async Task Initialize()
        {
            if (!await IsLoggedIn().ConfigureAwait(false))
                await NavigateToLogin();
        }

        private Task<bool> IsLoggedIn()
        {
            return auth.IsLoggedIn();
        }

        private async Task Logout()
        {
            log.Debug("Logging out");

            await auth.Logout().ConfigureAwait(false);
            await NavigateToLogin().ConfigureAwait(false);
        }

        private Task NavigateToLogin()
        {
            return navigation.NavigateToModal<LoginViewModel>();
        }
    }
}