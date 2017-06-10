using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeCredentialService : ICredentialService
    {
        public bool PasswordChanged { get; set; }

        Task ICredentialService.ChangePassword(string oldPassword, string newPassword)
        {
            PasswordChanged = true;

            return Task.FromResult(0);
        }
    }
}