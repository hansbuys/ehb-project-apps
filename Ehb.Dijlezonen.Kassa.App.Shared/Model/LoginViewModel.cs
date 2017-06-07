using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class LoginViewModel : PropertyChangedViewModelBase
    {
        private readonly ILoginProvider auth;
        private readonly ILog log;
        private string password;
        private string user;

        public LoginViewModel(ILoginProvider auth, Logging logging)
        {
            this.auth = auth;
            log = logging.GetLoggerFor<LoginViewModel>();

            LoginCommand = new Command(async () => await Login(), CanLogin);
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "Gebruikersnaam";
        public string PasswordPlaceholder => "Paswoord";
        public string LoginCommandText => "Log in";

        public Command LoginCommand { get; }

        public string User
        {
            get { return user; }
            set { Set(ref user, value, CredentialsChanged); }
        }

        public string Password
        {
            get { return password; }
            set { Set(ref password, value, CredentialsChanged); }
        }
        
        private void CredentialsChanged()
        {
            LoginCommand.ChangeCanExecute();
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task Login()
        {
            log.Debug("Attempting logging in");

            await auth.Login(User, Password);
        }
    }
}