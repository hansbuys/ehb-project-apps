using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IBackendClient
    {
        Task Login(string user, string password);
        Task Logout();

        Task ChangePassword(string oldPassword, string newPassword);

        User LoggedInUser { get; }
    }
}