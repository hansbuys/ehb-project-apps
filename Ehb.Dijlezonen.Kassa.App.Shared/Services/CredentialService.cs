using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly BackendClient client;

        public CredentialService(BackendClient client)
        {
            this.client = client;
        }

        async Task ICredentialService.ChangePassword(string oldPassword, string newPassword)
        {
            var response = await client.PostJson("/password/change", new
            {
                oldPassword,
                newPassword
            },
            true);

            response.EnsureSuccessStatusCode();
        }
    }
}