using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Microsoft.EntityFrameworkCore;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class IdentityResolver : IIdentityResolver
    {
        private readonly UserContext userContext;

        public IdentityResolver(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = await userContext.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Username == username).ConfigureAwait(false);
            if (user != null)
            {
                var claimsIdentity = new ClaimsIdentity(
                    new GenericIdentity(username, "Token"));

                var claims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)).ToList();

                if (user.AskNewPasswordOnNextLogin)
                    claims.Add(new Claim(CustomClaimTypes.NeedsPasswordChange, "true"));

                claimsIdentity.AddClaims(claims);

                return claimsIdentity;
            }

            // Account doesn't exists
            return null;
        }
    }
}