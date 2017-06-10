using System.Collections.Generic;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeCredentialService : ICredentialService
    {
        public bool PasswordChanged { get; set; }
        public ICollection<NewUserRegistration> Registrations { get; } = new List<NewUserRegistration>();

        Task ICredentialService.ChangePassword(string oldPassword, string newPassword)
        {
            PasswordChanged = true;

            return Task.FromResult(0);
        }

        Task ICredentialService.RegisterNewUser(NewUserRegistration registration)
        {
            Registrations.Add(registration);

            return Task.FromResult(0);
        }

        public class FakeUserRegistration
        {
            public FakeUserRegistration(string firstname, string lastname)
            {
                Firstname = firstname;
                Lastname = lastname;
            }

            public string Firstname { get; }
            public string Lastname { get; }
        }
    }
}