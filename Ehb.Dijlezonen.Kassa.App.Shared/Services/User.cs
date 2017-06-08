namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class User
    {
        public User(bool isAdmin = false, bool needsPasswordChange = false)
        {
            IsAdmin = isAdmin;
            NeedsPasswordChange = needsPasswordChange;
        }

        public bool IsAdmin { get; }
        public bool NeedsPasswordChange { get; }
    }
}