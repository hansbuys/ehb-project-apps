using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using System.Collections.Generic;
using System.Linq;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class UserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;
        private readonly Crypto crypto;
        private readonly ILogger<UserContextInitializer> log;

        public UserContextInitializer(UserContext context, Crypto crypto, ILogger<UserContextInitializer> log)
        {
            this.context = context;
            this.crypto = crypto;
            this.log = log;
        }

        public void Initialize()
        {
            //only initialize once, after this, the admin takes care of users
            //there should always be roles, so this is the best check we have available.
            if (!context.Roles.Any())
            {
                log.LogInformation("Seeding database.");

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