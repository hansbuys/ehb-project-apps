using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class LoginViewModel : UserInputViewModelBase
    {
        private readonly IAccountStore auth;
        private readonly ILog log;
        private readonly INavigationAdapter navigation;
        private string password;
        private string user;

        public LoginViewModel(IAccountStore auth, INavigationAdapter navigation, Logging logging)
        {
            this.auth = auth;
            this.navigation = navigation;
            log = logging.GetLoggerFor<LoginViewModel>();

            LoginCommand = new Command(async () => await Login().ConfigureAwait(false), CanLogin);
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "Gebruikersnaam";
        public string PasswordPlaceholder => "Paswoord";
        public string LoginCommandText => "Log in";

        public Command LoginCommand { get; }

        public string User
        {
            get => user;
            set => Set(ref user, value, CredentialsChanged);
        }

        public string Password
        {
            get => password;
            set => Set(ref password, value, CredentialsChanged);
        }


        private void CredentialsChanged()
        {
            LoginCommand.ChangeCanExecute();
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password);
        }

        public async Task Login()
        {
            log.Debug("Attempting logging in");

            if (await auth.Login(User, Password).ConfigureAwait(false))
            {
                log.Debug("Logged in success");
                await navigation.CloseModal().ConfigureAwait(false);
            }
            else
            {
                log.Debug("Login failed!");
            }
        }
    }
}