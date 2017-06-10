using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class UserService
    {
        private readonly IAuthenticationService authentication;
        private readonly ICredentialService credentials;
        private readonly ILog log;

        public UserService(IAuthenticationService authentication, ICredentialService credentials, Logging logging)
        {
            this.authentication = authentication;
            this.credentials = credentials;
            this.log = logging.GetLoggerFor<UserService>();
        }

        //maybe it's not the best idea to use events?
        public event EventHandler LoggedIn;
        public event EventHandler LoggedOut;
        public event EventHandler NeedsPasswordChange;
        public event EventHandler PasswordHasChanged;

        public async Task ChangePassword(string oldPassword, string newPassword)
        {
            log.Debug("Attempting to change password.");

            try
            {
                await credentials.ChangePassword(oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                log.Error("Unable to change password.", ex);
                throw;
            }

            log.Debug("Password has been successfully changed.");

            PasswordHasChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task Login(string user, string password)
        {
            log.Debug("Attempting logging in.");

            try
            {
                await authentication.Login(user, password);
            }
            catch (Exception ex)
            {
                log.Error("Unable to login.", ex);
                throw;
            }

            log.Debug($"{user} has succesfully logging in.");

            LoggedIn?.Invoke(this, EventArgs.Empty);

            if (authentication.LoggedInUser != null && authentication.LoggedInUser.NeedsPasswordChange)
            {
                log.Debug($"{user} needs to change passwords.");

                NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task Logout()
        {
            log.Debug("User is logging out");

            await authentication.Logout();

            log.Debug("User has logged out");

            LoggedOut?.Invoke(this, EventArgs.Empty);
        }
    }
}