using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement
{
    public class LoginViewModel : PropertyChangedViewModelBase
    {
        private readonly UserService userService;
        private readonly ILog log;

        public LoginViewModel(UserService userService, Logging logging)
        {
            this.userService = userService;
            log = logging.GetLoggerFor<LoginViewModel>();

            LoginCommand = new Command(async () => await Login(), CanLogin);
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "Gebruikersnaam";
        public string PasswordPlaceholder => "Paswoord";
        public string LoginCommandText => "Log in";

        public Command LoginCommand { get; }

        private string user;
        public string User
        {
            get { return user; }
            set { Set(ref user, value, CredentialsChanged); }
        }

        private string password;
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
            await userService.Login(User, Password);
        }
    }
}