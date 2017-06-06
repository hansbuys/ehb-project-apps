using System.Security.Claims;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public interface IIdentityResolver
    {
        Task<ClaimsIdentity> GetIdentity(string username, string password);
    }
}