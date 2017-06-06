using System;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface ILoginProvider
    {
        Task<bool> IsLoggedIn();
        Task Login(string user, string password);
        Task Logout();

        event EventHandler LoggedOut;
        event EventHandler LoggedIn;
        event EventHandler NeedsPasswordChange;

        Token Token { get; }
        Task ChangePassword(string newPassword);
    }

    public class Token
    {
        public Token(string value, DateTime expiration, bool isAdmin = false, bool needsPasswordChange = false)
        {
            Value = value;
            Expiration = expiration;
            IsAdmin = isAdmin;
            NeedsPasswordChange = needsPasswordChange;
        }

        public DateTime Expiration { get; }
        public string Value { get; }

        public bool IsValid => !string.IsNullOrEmpty(Value) && Expiration > DateTime.UtcNow;
        public bool IsAdmin { get; }
        public bool NeedsPasswordChange { get; }
    }
}