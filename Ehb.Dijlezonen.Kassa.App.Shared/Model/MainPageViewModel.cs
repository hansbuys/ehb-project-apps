using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel : PropertyChangedViewModelBase, IRequireLogin, IDisposable
    {
        private readonly UserService userService;
        private readonly Navigation navigation;

        private readonly EventHandler onLoggedIn;

        public MainPageViewModel(UserService userService, IAuthentication client, Navigation navigation)
        {
            this.userService = userService;
            this.navigation = navigation;

            onLoggedIn = (s, a) => IsAdmin = client.LoggedInUser?.IsAdmin ?? false;
            userService.LoggedIn += onLoggedIn;
        }

        public string Title => "De Dijlezonen Kassa";

        public string LogoutCommandText => "Uitloggen";
        public ICommand LogoutCommand => new Command(async () => await Logout());
        public string NavigateToAdminCommandText => "Administration";
        public ICommand NavigateToAdminCommand => new Command(async () => { await NavigateToAdminOverview(); });
        
        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { Set(ref isAdmin, value); }
        }

        private Task Logout()
        {
            return userService.Logout();
        }

        private Task NavigateToAdminOverview()
        {
            return navigation.NavigateTo<OverviewViewModel>();
        }

        public void Dispose()
        {
            userService.LoggedIn -= onLoggedIn;
        }
    }
}