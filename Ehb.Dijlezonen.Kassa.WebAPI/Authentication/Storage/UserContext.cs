using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options, ILogger<UserContext> logger)
            : base(options)
        {
            logger.LogDebug("Created new UserContext");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
