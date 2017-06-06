using System.Security.Claims;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class IdentityResolver : IIdentityResolver
    {
        public Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // Account doesn't exists
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}