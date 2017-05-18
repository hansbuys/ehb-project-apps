using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IAccountStore
    {
        Task<bool> IsLoggedIn();
        Task<bool> Login(string user, string password);
        Task Logout();
    }
}