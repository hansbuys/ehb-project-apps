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

        Task ICredentialService.ChangePassword(string oldPassword, string newPassword)
        {
            return client.PostJson("/password/change", new
            {
                oldPassword,
                newPassword
            });
        }

        Task ICredentialService.RegisterNewUser(NewUserRegistration registration)
        {
            return client.PostJson("/user/create", new
            {
                registration.Username,
                registration.Password,
                registration.Firstname,
                registration.Lastname,
                registration.IsBlocked,
                AskNewPasswordOnNextLogin = registration.NeedsPasswordChange,
                Roles = new[]
                {
                    new
                    {
                        Name = "User"
                    }
                }
            });
        }
    }
}