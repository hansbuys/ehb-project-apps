using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model
{
    public class User
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }

        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public bool AskNewPasswordOnNextLogin { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        [NotMapped]
        public IEnumerable<Role> Roles
        {
            get { return UserRoles.Select(x => x.Role); }
            set { UserRoles = value.Select(x => new UserRole(Id, x.Id)).ToList(); }
        }
    }
}