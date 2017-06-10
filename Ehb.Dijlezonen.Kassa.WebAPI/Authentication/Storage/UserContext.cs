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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.UserRoles);

            base.OnModelCreating(modelBuilder);
        }
    }
}
