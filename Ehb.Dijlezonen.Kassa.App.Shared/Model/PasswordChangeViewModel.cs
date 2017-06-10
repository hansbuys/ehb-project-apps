using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class PasswordChangeViewModel : PropertyChangedViewModelBase
    {
        private readonly UserService userService;

        public PasswordChangeViewModel(UserService userService)
        {
            this.userService = userService;

            ChangePasswordCommand = new Command(
                async () => await ChangePassword(),
                CanChangePassword);
        }

        private bool CanChangePassword()
        {
            return !string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmNewPassword;
        }

        private Task ChangePassword()
        {
            return userService.ChangePassword(OldPassword, NewPassword);
        }

        public string Title => "Verander je paswoord aub.";

        public string OldPasswordPlaceholder => "Oud password";
        public string NewPasswordPlaceholder => "Nieuw password";
        public string ConfirmNewPasswordPlaceholder => "Herhaal nieuw paswoord";
        public string ChangePasswordCommandText => "Verander paswoord";

        public Command ChangePasswordCommand { get; }

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

        private string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set { Set(ref oldPassword, value, PasswordsChanged); }
        }

        private void PasswordsChanged()
        {
            ChangePasswordCommand.ChangeCanExecute();
        }
    }
}