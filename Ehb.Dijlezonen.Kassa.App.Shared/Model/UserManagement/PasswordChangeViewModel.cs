using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement
{
    public class PasswordChangeViewModel : PropertyChangedViewModelBase
    {
        private readonly ICredentialService credentials;
        private readonly INavigationAdapter navigation;

        public PasswordChangeViewModel(ICredentialService credentials, INavigationAdapter navigation, Logging logging)
        {
            this.credentials = credentials;
            this.navigation = navigation;
            log = logging.GetLoggerFor<PasswordChangeViewModel>();

            ChangePasswordCommand = new Command(
                async () => await ChangePassword(),
                CanChangePassword);
        }

        private bool CanChangePassword()
        {
            return !string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmNewPassword;
        }

        private async Task ChangePassword()
        {
            log.Debug("Attempting to change password.");

            try
            {
                await credentials.ChangePassword(OldPassword, NewPassword);
            }
            catch (Exception ex)
            {
                log.Error("Unable to change password.", ex);
                throw;
            }

            log.Debug("Password has been successfully changed.");
            await navigation.CloseModal();
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
        private ILog log;

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