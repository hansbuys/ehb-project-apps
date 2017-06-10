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
            try
            {
                await credentials.ChangePassword(oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                log.Error("Unable to change password", ex);
                throw;
            }

            PasswordHasChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task Login(string user, string password)
        {
            try
            {
                await authentication.Login(user, password);
            }
            catch (Exception ex)
            {
                log.Error("Unable to login", ex);
                throw;
            }

            LoggedIn?.Invoke(this, EventArgs.Empty);

            if (authentication.LoggedInUser != null && authentication.LoggedInUser.NeedsPasswordChange)
                NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
        }

        public async Task Logout()
        {
            log.Debug("Logging out");

            await authentication.Logout();

            LoggedOut?.Invoke(this, EventArgs.Empty);
        }
    }
}