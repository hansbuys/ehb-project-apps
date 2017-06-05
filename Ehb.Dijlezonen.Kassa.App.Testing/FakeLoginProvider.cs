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
                IsLoggedIn = true;

            return Task.FromResult(0);
        }

        Task ILoginProvider.Logout()
        {
            IsLoggedIn = false;
            return Task.FromResult(0);
        }

        public void WhenUserIsLoggedIn()
        {
            IsLoggedIn = true;
        }

        public void WhenUserIsKnown(string user, string pass)
        {
            KnownUsers.AddOrUpdate(user, s => pass, (s, s1) => { throw new Exception("User is already known"); });
        }
    }
}