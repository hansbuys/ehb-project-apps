using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.EntityFrameworkCore;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserContext userContext;
        private readonly ILogger<IdentityRepository> log;
        private readonly Crypto crypto;

        public IdentityRepository(UserContext userContext, ILogger<IdentityRepository> log, Crypto crypto)
        {
            this.userContext = userContext;
            this.log = log;
            this.crypto = crypto;
        }

        public async Task<Identity> GetIdentity(string username, string password)
        {
            log.LogDebug($"fetching user {username}.");

            var user = await userContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(u => u.Role)
                .SingleOrDefaultAsync(u => 
                    u.Username == username && 
                    !u.IsBlocked)
                .ConfigureAwait(false);

            if (AccountDoesNotExist(password, user))
                return null;

            log.LogDebug($"found user '{username}' with id '{user.Id}'.");

            if (user.AskNewPasswordOnNextLogin)
            {
                log.LogDebug($"{user.Username} needs to reset their password.");
            }

            return new Identity(
                user.Username,
                user.Roles.Select(x => x.Name),
                user.AskNewPasswordOnNextLogin);
        }

        private bool AccountDoesNotExist(string password, User user)
        {
            return user == null || !crypto.Verify(user.Password, password);
        }
    }
}