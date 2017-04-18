using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAccountStore auth;
        private readonly NavigationService navigation;

        public LoginViewModel(IAccountStore auth, NavigationService navigation)
        {
            this.auth = auth;
            this.navigation = navigation;

            LoginCommand = new Command(async () => await Login().ConfigureAwait(false), CanLogin);
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "User";
        private string user;
        public string User
        {
            get { return user; }
            set { Set(ref user, value, UserChanged); }
        }
        private void UserChanged()
        {
            LoginCommand.ChangeCanExecute();
        }

        public string PasswordPlaceholder => "Password";
        private string password;
        public string Password
        {
            get { return password; }
            set { Set(ref password, value, PasswordChanged); }
        }
        private void PasswordChanged()
        {
            LoginCommand.ChangeCanExecute();
        }

        public string LoginCommandText => "Password";
        public Command LoginCommand { get; }
        
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password);
        }

        public async Task Login()
        {
            if (await auth.Login(User, Password).ConfigureAwait(false))
            {
                await navigation.GoToMain().ConfigureAwait(false);
            }
        }
    }
}
