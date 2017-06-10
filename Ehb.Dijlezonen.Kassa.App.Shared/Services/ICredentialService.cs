using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface ICredentialService
    {
        Task ChangePassword(string oldPassword, string newPassword);
        Task RegisterNewUser(NewUserRegistration registration);
    }

    public class NewUserRegistration
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }

        public bool IsBlocked { get; set; }
        public bool NeedsPasswordChange { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}