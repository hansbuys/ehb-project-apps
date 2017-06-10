using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IAuthenticationService
    {
        Task Login(string user, string password);
        Task Logout();

        User LoggedInUser { get; }
    }
}