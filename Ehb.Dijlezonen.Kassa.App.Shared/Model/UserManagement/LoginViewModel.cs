using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement
{
    public class LoginViewModel : PropertyChangedViewModelBase, IDisposable
    {
        private readonly IAuthentication authentication;
        private readonly INavigationAdapter navigation;
        private readonly ILog log;
        private readonly EventHandler onLoggedIn;

        public LoginViewModel(IAuthentication authentication, Logging logging, INavigationAdapter navigation)
        {
            this.authentication = authentication;
            this.navigation = navigation;
            log = logging.GetLoggerFor<LoginViewModel>();

            onLoggedIn = async (sender, args) => await OnLoggedIn();
            authentication.LoggedIn += onLoggedIn;

            LoginCommand = new Command(async () => await Login(), CanLogin);
        }

        private Task OnLoggedIn()
        {
            return navigation.CloseModal();
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "Gebruikersnaam";
        public string PasswordPlaceholder => "Paswoord";
        public string LoginCommandText => "Log in";


        private bool disableChangePasswordCommand;
        public bool DisableChangePasswordCommand
        {
            get => disableChangePasswordCommand;
            set => Set(ref disableChangePasswordCommand, value, UpdateLoginCommandAccess);
        }

        public Command LoginCommand { get; }

        private string user;
        public string User
        {
            get => user;
            set => Set(ref user, value, UpdateLoginCommandAccess);
        }

        private string password;
        public string Password
        {
            get => password;
            set => Set(ref password, value, UpdateLoginCommandAccess);
        }
        
        private void UpdateLoginCommandAccess()
        {
            LoginCommand.ChangeCanExecute();
        }

        private bool CanLogin()
        {
            return !DisableChangePasswordCommand && AllFieldsValidate();
        }

        private bool AllFieldsValidate()
        {
            return
                !string.IsNullOrWhiteSpace(User) &&
                !string.IsNullOrWhiteSpace(Password);
        }

        private async Task Login()
        {
            log.Debug("Attempting logging in.");
            DisableChangePasswordCommand = true;

            try
            {
                await authentication.Login(User, Password);
            }
            catch (Exception ex)
            {
                log.Error("Unable to login.", ex);
                DisableChangePasswordCommand = false;
                throw;
            }

            log.Debug("Logged in succesfully.");
        }

        public void Dispose()
        {
            authentication.LoggedIn -= onLoggedIn;
        }
    }
}