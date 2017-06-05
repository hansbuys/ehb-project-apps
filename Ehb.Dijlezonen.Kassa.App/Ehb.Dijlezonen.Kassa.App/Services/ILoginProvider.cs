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

        Token Token { get; }
    }

    public class Token
    {
        public Token(string value, DateTime expiration, bool isAdmin = false)
        {
            Value = value;
            Expiration = expiration;
            IsAdmin = isAdmin;
        }

        public DateTime Expiration { get; }
        public string Value { get; }

        public bool IsValid => !string.IsNullOrEmpty(Value) && Expiration > DateTime.UtcNow;
        public bool IsAdmin { get; }
    }
}