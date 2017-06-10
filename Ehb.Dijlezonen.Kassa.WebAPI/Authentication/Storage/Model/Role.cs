using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsAdminRole { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}