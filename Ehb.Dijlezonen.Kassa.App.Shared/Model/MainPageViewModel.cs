using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel : PropertyChangedViewModelBase, IRequireAuthentication, IDisposable
    {
        private readonly IAuthentication client;
        private readonly Navigation navigation;
        private readonly ILog log;

        private readonly EventHandler onLoggedIn;

        public MainPageViewModel(IAuthentication client, Navigation navigation, Logging logging)
        {
            this.client = client;
            this.navigation = navigation;
            this.log = logging.GetLoggerFor<MainPageViewModel>();
            
            onLoggedIn = (s, a) => UpdateIsAdmin();
            client.LoggedIn += onLoggedIn;

            NavigateToAdminCommand = new Command(async () => { await NavigateToAdminOverview(); }, () => IsAdmin);
            LogoutCommand = new Command(async () => await Logout());

            UpdateIsAdmin();
        }

        private void UpdateIsAdmin()
        {
            var admin = client.LoggedInUser?.IsAdmin;
            IsAdmin = admin.HasValue && admin.Value;
        }

        public string Title => "De Dijlezonen Kassa";

        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set => Set(ref isAdmin, value, UpdateNavigateToAdminAccess);
        }

        public string LogoutCommandText => "Uitloggen";
        public Command LogoutCommand { get; }

        public string NavigateToAdminCommandText => "Administration";
        public Command NavigateToAdminCommand { get; }

        private void UpdateNavigateToAdminAccess()
        {
            NavigateToAdminCommand.ChangeCanExecute();
        }

        private async Task Logout()
        {
            log.Debug("User is logging out");

            await client.Logout();

            log.Debug("User has logged out");
        }

        private Task NavigateToAdminOverview()
        {
            return navigation.NavigateTo<OverviewViewModel>();
        }

        public void Dispose()
        {
            client.LoggedIn -= onLoggedIn;
        }
    }
}