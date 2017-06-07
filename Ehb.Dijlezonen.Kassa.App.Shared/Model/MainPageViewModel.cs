using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel : PropertyChangedViewModelBase, IProtectedViewModel, IDisposable
    {
        private readonly ILoginProvider auth;
        private readonly ILog log;

        public MainPageViewModel(ILoginProvider auth, Logging logging)
        {
            this.auth = auth;
            log = logging.GetLoggerFor<MainPageViewModel>();

            onLoggedIn = (s, a) => IsAdmin = auth.Token != null && auth.Token.IsAdmin;
            auth.LoggedIn += onLoggedIn;
        }

        public string Title => "De Dijlezonen Kassa";

        public string LogoutCommandText => "Uitloggen";
        public ICommand LogoutCommand => new Command(async () => await Logout().ConfigureAwait(false));
        public string NavigateToAdminCommandText => "Administration";
        public ICommand NavigateToAdminCommand => new Command(() => { /*does nothing yet*/ });

        private bool isAdmin;
        private EventHandler onLoggedIn;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { Set(ref isAdmin, value); }
        }

        private async Task Logout()
        {
            log.Debug("Logging out");

            await auth.Logout().ConfigureAwait(false);
        }

        public void Dispose()
        {
            auth.LoggedIn -= onLoggedIn;
        }
    }
}