using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using System.Collections.Generic;
using System.Linq;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class UserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;

        public UserContextInitializer(UserContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            var userRole = new Role { Name = "User" };
            var adminRole = new Role { Name = "Admin", IsAdminRole = true };

            if (!context.Roles.Any())
                context.Roles.AddRange(
                    userRole,
                    adminRole);

            if (!context.Users.Any())
                context.Users.AddRange(
                    new User
                    {
                        Username = "beheerder",
                        Password = "beheerder-paswoord",
                        Salt = "beheerder-salt",
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
                        Password = "gebruiker-paswoord",
                        Salt = "gebruiker-salt",
                        AskNewPasswordOnNextLogin = true,
                        Roles = new List<Role>
                        {
                            userRole
                        }
                    });
        }
    }
}