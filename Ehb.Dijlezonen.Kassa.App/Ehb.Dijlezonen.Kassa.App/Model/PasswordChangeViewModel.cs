using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class PasswordChangeViewModel : PropertyChangedViewModelBase
    {
        private readonly ILoginProvider login;

        public PasswordChangeViewModel(ILoginProvider login)
        {
            this.login = login;
        }

        private bool CanChangePassword()
        {
            return !string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmNewPassword;
        }

        private Task ChangePassword()
        {
            return login.ChangePassword(NewPassword);
        }

        public string Title => "Verander je paswoord aub.";

        public string NewPasswordPlaceholder => "Nieuw password";
        public string ConfirmNewPasswordPlaceholder => "Herlaal nieuw paswoord";
        public string ChangePasswordCommandText => "Verander paswoord";

        public Command ChangePasswordCommand => new Command(async () => await ChangePassword().ConfigureAwait(false), CanChangePassword);

        private string newPassword;
        public string NewPassword
        {
            get { return newPassword; }
            set { Set(ref newPassword, value, PasswordsChanged); }
        }

        private string confirmNewPassword;

        public string ConfirmNewPassword
        {
            get { return confirmNewPassword; }
            set { Set(ref confirmNewPassword, value, PasswordsChanged); }
        }

        private void PasswordsChanged()
        {
            ChangePasswordCommand.ChangeCanExecute();
        }
    }
}