using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel : PropertyChangedViewModelBase, IProtectedViewModel, IDisposable
    {
        private readonly UserService userService;

        public MainPageViewModel(UserService userService, IBackendClient client)
        {
            this.userService = userService;

            onLoggedIn = (s, a) => IsAdmin = client.LoggedInUser?.IsAdmin ?? false;
            userService.LoggedIn += onLoggedIn;
        }

        public string Title => "De Dijlezonen Kassa";

        public string LogoutCommandText => "Uitloggen";
        public ICommand LogoutCommand => new Command(async () => await Logout());
        public string NavigateToAdminCommandText => "Administration";
        public ICommand NavigateToAdminCommand => new Command(() => { /*does nothing yet*/ });

        private bool isAdmin;
        private readonly EventHandler onLoggedIn;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { Set(ref isAdmin, value); }
        }

        private Task Logout()
        {
            return userService.Logout();
        }

        public void Dispose()
        {
            userService.LoggedIn -= onLoggedIn;
        }
    }
}