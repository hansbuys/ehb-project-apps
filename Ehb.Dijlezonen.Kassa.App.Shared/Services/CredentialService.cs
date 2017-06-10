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
            });

            response.EnsureSuccessStatusCode();
        }

        async Task ICredentialService.RegisterNewUser(NewUserRegistration registration)
        {
            var response = await client.PostJson("/user/create", new
            {
                registration.Username,
                registration.Password,
                AskNewPasswordOnNextLogin = registration.NeedsPasswordChange,
                Roles = new[]
                {
                    new
                    {
                        Name = "User"
                    }
                }
            });

            response.EnsureSuccessStatusCode();
        }
    }
}