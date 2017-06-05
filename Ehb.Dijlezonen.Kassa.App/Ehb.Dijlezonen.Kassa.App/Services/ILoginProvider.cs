using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface ILoginProvider
    {
        Task<bool> IsLoggedIn();
        Task Login(string user, string password);
        Task Logout();
    }
}