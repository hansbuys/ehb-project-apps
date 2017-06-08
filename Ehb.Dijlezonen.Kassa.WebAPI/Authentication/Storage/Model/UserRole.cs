namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage.Model
{
    public class UserRole
    {
        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        protected UserRole() { }

        public User User { get; set; }
        public Role Role { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}