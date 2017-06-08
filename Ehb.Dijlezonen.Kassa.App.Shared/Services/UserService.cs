using System;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class UserService
    {
        private readonly IBackendClient client;
        private readonly ILog log;

        public UserService(IBackendClient client, Logging logging)
        {
            this.client = client;
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
                await client.ChangePassword(oldPassword, newPassword);
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
                await client.Login(user, password);
            }
            catch (Exception ex)
            {
                log.Error("Unable to login", ex);
                throw;
            }

            LoggedIn?.Invoke(this, EventArgs.Empty);

            if (client.LoggedInUser?.NeedsPasswordChange ?? false)
                NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
        }

        public async Task Logout()
        {
            log.Debug("Logging out");

            await client.Logout();

            LoggedOut?.Invoke(this, EventArgs.Empty);
        }
    }
}