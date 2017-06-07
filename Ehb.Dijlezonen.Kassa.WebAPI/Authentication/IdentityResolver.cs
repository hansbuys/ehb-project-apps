using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.EntityFrameworkCore;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class IdentityResolver : IIdentityResolver
    {
        private readonly UserContext userContext;
        private readonly ILogger<IdentityResolver> log;
        private readonly Crypto crypto;

        public IdentityResolver(UserContext userContext, ILogger<IdentityResolver> log, Crypto crypto)
        {
            this.userContext = userContext;
            this.log = log;
            this.crypto = crypto;
        }

        public async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            log.LogDebug($"fetching user {username}.");
            var user = await userContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Username == username).ConfigureAwait(false);
            log.LogDebug($"found user with id '{user?.Id}'.");

            if (user != null && crypto.Verify(user.Password, password))
            {
                log.LogDebug($"creating ClaimsIdentity for {user.Username}.");
                var claimsIdentity = new ClaimsIdentity(
                    new GenericIdentity(username, "Token"));

                var roles = string.Join(", ", user.Roles.Select(x => x.Name));
                log.LogDebug($"user has roles {roles}.");

                var claims = user.Roles.Select(r => new Claim(CustomClaimTypes.Role, r.Name)).ToList();

                if (user.AskNewPasswordOnNextLogin)
                {
                    log.LogDebug($"{user.Username} needs to reset their password.");
                    claims.Add(new Claim(CustomClaimTypes.NeedsPasswordChange, "true"));
                }

                claimsIdentity.AddClaims(claims);

                return claimsIdentity;
            }

            // Account doesn't exists
            return null;
        }
    }
}