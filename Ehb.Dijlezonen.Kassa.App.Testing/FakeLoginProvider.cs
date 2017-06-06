using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeLoginProvider : ILoginProvider
    {
        public bool IsLoggedIn { get; set; }
        public ConcurrentDictionary<string, string> KnownUsers { get; } = new ConcurrentDictionary<string, string>();

        Task<bool> ILoginProvider.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn);
        }

        Task ILoginProvider.Login(string user, string password)
        {
            var isKnownUser = KnownUsers.ContainsKey(user) && KnownUsers[user] == password;

            if (isKnownUser)
            {
                WhenUserIsLoggedIn();
            }

            return Task.FromResult(0);
        }

        Task ILoginProvider.Logout()
        {
            IsLoggedIn = false;
            OnLoggedOut();
            return Task.FromResult(0);
        }

        public event EventHandler LoggedOut;
        public event EventHandler LoggedIn;

        public Token Token => null;

        public void WhenUserIsLoggedIn()
        {
            OnLoggedIn();
            IsLoggedIn = true;
        }

        public void WhenUserIsKnown(string user, string pass)
        {
            KnownUsers.AddOrUpdate(user, s => pass, (s, s1) => { throw new Exception("User is already known"); });
        }

        protected virtual void OnLoggedOut()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoggedIn()
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }
    }
}