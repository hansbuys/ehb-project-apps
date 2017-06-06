using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class FakeIdentityResolver : IIdentityResolver
    {
        public Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // DO NOT USE IN PRODUCTION!!!
            if (username == "user" && password == "resu321")
            {
                var claimsIdentity = new ClaimsIdentity(new GenericIdentity(username, "Token"));
                return Task.FromResult(claimsIdentity);
            }
            if(username == "admin" && password == "nimda321")
            {
                var claimsIdentity = new ClaimsIdentity(
                    new GenericIdentity(username, "Token"));

                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

                return Task.FromResult(claimsIdentity);
            }

            // Account doesn't exists
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}