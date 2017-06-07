using System.Collections.Generic;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool AskNewPasswordOnNextLogin { get; set; }

        public virtual List<Role> Roles { get; set; }
    }
}