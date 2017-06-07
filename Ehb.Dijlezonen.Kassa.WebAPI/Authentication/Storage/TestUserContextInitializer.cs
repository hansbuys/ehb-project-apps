using System.Collections.Generic;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class TestUserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;
        private readonly Crypto crypto;
        private readonly ILogger<TestUserContextInitializer> log;

        public TestUserContextInitializer(UserContext context, Crypto crypto, ILogger<TestUserContextInitializer> log)
        {
            this.context = context;
            this.crypto = crypto;
            this.log = log;
        }

        public void Initialize()
        {
            log.LogDebug("Clearing roles & users");
            context.Roles.RemoveRange(context.Roles);
            context.Users.RemoveRange(context.Users);
            
            var userRole = new Role {Name = "User"};
            var adminRole = new Role {Name = "Admin", IsAdminRole = true};

            log.LogDebug("Recreating roles");
            context.Roles.AddRange(
                userRole,
                adminRole);

            var adminPass = crypto.Encrypt("beheerder");
            var userPass = crypto.Encrypt("gebruiker");

            log.LogDebug("Recreating users");
            log.LogDebug($"admin pass: {adminPass}");
            log.LogDebug($"user pass: {userPass}");
            context.Users.AddRange(
                new User
                {
                    Username = "beheerder",
                    Password = adminPass,
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
                    Password = userPass,
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