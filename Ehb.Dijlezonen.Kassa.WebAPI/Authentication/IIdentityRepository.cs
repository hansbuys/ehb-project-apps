using System.Security.Claims;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public interface IIdentityRepository
    {
        Task<Identity> GetIdentity(string username, string password);
    }
}