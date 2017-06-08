namespace Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin
{
    public class AddUserViewModel : PropertyChangedViewModelBase, IRequireAdminPrivileges
    {
        public string Title => "Nieuwe gebruiker";
        public string Subtitle => "Maak een nieuwe gebruiker";


        public string EmailAddressPlaceholder => "Email adres";
        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set { Set(ref emailAddress, value); }
        }

        public string FirstnamePlaceholder => "Voornaam";
        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set { Set(ref firstname, value); }
        }

        public string LastnamePlaceholder => "Achternaam";
        private string lastname;
        public string Lastname
        {
            get { return lastname; }
            set { Set(ref lastname, value); }
        }

        public string PasswordPlaceholder => "Wachtwoord";
        private string password;
        public string Password
        {
            get { return password; }
            set { Set(ref password, value); }
        }

        public string IsBlockedLabel => "Geblokkeerd?";
        private bool isBlocked;
        public bool IsBlocked
        {
            get { return isBlocked; }
            set { Set(ref isBlocked, value); }
        }

        public string PasswordNeedsResetLabel => "Wachtwoord herinstellen?";
        private bool passwordNeedsReset;
        public bool PasswordNeedsReset
        {
            get { return passwordNeedsReset; }
            set { Set(ref passwordNeedsReset, value); }
        }
    }
}
