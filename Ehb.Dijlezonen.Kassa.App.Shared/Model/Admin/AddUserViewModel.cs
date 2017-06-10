using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin
{
    public class AddUserViewModel : PropertyChangedViewModelBase, IRequireAdminPrivileges
    {
        private readonly ILog log;
        private readonly INavigationAdapter navigation;
        private readonly ICredentialService credential;

        public AddUserViewModel(Logging logging, INavigationAdapter navigation, ICredentialService credential)
        {
            this.navigation = navigation;
            this.credential = credential;
            log = logging.GetLoggerFor<AddUserViewModel>();

            AddNewUserCommand = new Command(async () => await AddNewUser(), CanAddNewUser);
        }

        public string Title => "Nieuwe gebruiker";
        public string Subtitle => "Maak een nieuwe gebruiker";

        public string EmailAddressPlaceholder => "Email adres";

        private string emailAddress;
        public string EmailAddress
        {
            get => emailAddress;
            set => Set(ref emailAddress, value, UpdateAddNewUserAccess);
        }

        public string FirstnamePlaceholder => "Voornaam";

        private string firstname;
        public string Firstname
        {
            get => firstname;
            set => Set(ref firstname, value, UpdateAddNewUserAccess);
        }

        public string LastnamePlaceholder => "Achternaam";

        private string lastname;
        public string Lastname
        {
            get => lastname;
            set => Set(ref lastname, value, UpdateAddNewUserAccess);
        }

        public string PasswordPlaceholder => "Wachtwoord";

        private string password;
        public string Password
        {
            get => password;
            set => Set(ref password, value, UpdateAddNewUserAccess);
        }

        public string IsBlockedLabel => "Geblokkeerd?";

        private bool isBlocked;
        public bool IsBlocked
        {
            get => isBlocked;
            set => Set(ref isBlocked, value);
        }

        public string PasswordNeedsResetLabel => "Wachtwoord herinstellen?";

        private bool passwordNeedsReset;
        public bool PasswordNeedsReset
        {
            get => passwordNeedsReset;
            set => Set(ref passwordNeedsReset, value);
        }

        private bool forceDisableAddNewUser;

        public bool ForceDisableAddNewUser
        {
            get => forceDisableAddNewUser;
            set => Set(ref forceDisableAddNewUser, value, UpdateAddNewUserAccess);
        }

        private void UpdateAddNewUserAccess()
        {
            AddNewUserCommand.ChangeCanExecute();
        }

        private bool CanAddNewUser()
        {
            return !ForceDisableAddNewUser &&
                !string.IsNullOrEmpty(Firstname) &&
                !string.IsNullOrEmpty(Lastname) &&
                !string.IsNullOrEmpty(EmailAddress) &&
                !string.IsNullOrEmpty(Password);
        }

        private async Task AddNewUser()
        {
            ForceDisableAddNewUser = true;

            try
            {
                await credential.RegisterNewUser(new NewUserRegistration
                {
                    Username = EmailAddress,
                    Firstname = Firstname,
                    Lastname = Lastname,
                    Password = Password,
                    IsBlocked = IsBlocked,
                    NeedsPasswordChange = PasswordNeedsReset,
                    Roles = new[]
                    {
                        "User"
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("New user registration failed.", ex);
                ForceDisableAddNewUser = false;
                return;
            }

            await navigation.Close();
        }

        public string AddNewUserCommandText => "Registreer";
        public Command AddNewUserCommand { get; }
    }
}