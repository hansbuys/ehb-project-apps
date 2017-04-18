using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class LoginViewModel : UserInputViewModelBase
    {
        private readonly IAccountStore auth;
        private readonly INavigationAdapter navigation;

        public LoginViewModel(IAccountStore auth, INavigationAdapter navigation)
        {
            this.auth = auth;
            this.navigation = navigation;

            LoginCommand = new Command(async () => await Login().ConfigureAwait(false), CanLogin);
        }

        public string Title => "Log in aub.";

        public string UserPlaceholder => "Gebruikersnaam";
        private string user;
        public string User
        {
            get { return user; }
            set { Set(ref user, value, CredentialsChanged); }
        }
        private void CredentialsChanged()
        {
            LoginCommand.ChangeCanExecute();
        }

        public string PasswordPlaceholder => "Paswoord";
        private string password;
        public string Password
        {
            get { return password; }
            set { Set(ref password, value, CredentialsChanged); }
        }

        public string LoginCommandText => "Log in";
        public Command LoginCommand { get; }
        
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(User) && !string.IsNullOrWhiteSpace(Password);
        }

        public async Task Login()
        {
            if (await auth.Login(User, Password).ConfigureAwait(false))
            {
                await navigation.CloseModal().ConfigureAwait(false);
            }
        }
    }
}
