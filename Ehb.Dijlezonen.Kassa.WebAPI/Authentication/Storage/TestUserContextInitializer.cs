using System.Collections.Generic;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class TestUserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;
        private readonly Crypto crypto;

        public TestUserContextInitializer(UserContext context, Crypto crypto)
        {
            this.context = context;
            this.crypto = crypto;
        }

        public void Initialize()
        {
            context.Roles.RemoveRange(context.Roles);
            context.Users.RemoveRange(context.Users);
            
            var userRole = new Role {Name = "User"};
            var adminRole = new Role {Name = "Admin", IsAdminRole = true};

            context.Roles.AddRange(
                userRole,
                adminRole);

            var adminPass = crypto.Encrypt("beheerder");
            var userPass = crypto.Encrypt("gebruiker");

            context.Users.AddRange(
                new User
                {
                    Username = "beheerder",
                    Password = adminPass.Password,
                    Salt = adminPass.Salt,
                    AskNewPasswordOnNextLogin = true,
                    Roles = new List<Role>
                    {
                        userRole,
                        adminRole
                    }
                },
                new User
                {
                    Username = "gebruiker",
                    Password = userPass.Password,
                    Salt = userPass.Salt,
                    AskNewPasswordOnNextLogin = true,
                    Roles = new List<Role>
                    {
                        userRole
                    }
                });

            context.SaveChanges();
        }
    }
}