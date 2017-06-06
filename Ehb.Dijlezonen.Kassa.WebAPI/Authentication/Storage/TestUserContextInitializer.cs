using System.Collections.Generic;
using System.Linq;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class TestUserContextInitializer : IDbContextInitializer
    {
        private readonly UserContext context;

        public TestUserContextInitializer(UserContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {

            if (!context.Roles.Any())
            {
                var userRole = new Role {Name = "User"};
                var adminRole = new Role {Name = "Admin", IsAdminRole = true};

                context.Roles.AddRange(
                    userRole,
                    adminRole);

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

                context.SaveChanges();
            }
        }
    }
}