using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface ICredentialService
    {
        Task ChangePassword(string oldPassword, string newPassword);
    }
}