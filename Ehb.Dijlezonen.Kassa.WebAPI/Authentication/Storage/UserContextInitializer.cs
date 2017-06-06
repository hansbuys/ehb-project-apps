using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using System.Collections.Generic;
using System.Linq;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class UserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;
        private readonly Crypto crypto;

        public UserContextInitializer(UserContext context, Crypto crypto)
        {
            this.context = context;
            this.crypto = crypto;
        }

        public void Initialize()
        {
            //only initialize once, after this, the admin takes care of users
            //there should always be roles, so this is the best check we have available.
            if (!context.Roles.Any())
            {
                var userRole = new Role { Name = "User" };
                var adminRole = new Role { Name = "Admin", IsAdminRole = true };

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
            }
        }
    }
}