using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeAccountStore : IAccountStore
    {
        public bool IsLoggedIn { get; set; }
        public ConcurrentDictionary<string, string> KnownUsers { get; } = new ConcurrentDictionary<string, string>();

        Task<bool> IAccountStore.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn);
        }

        Task<bool> IAccountStore.Login(string user, string password)
        {
            var isKnownUser = KnownUsers.ContainsKey(user) && KnownUsers[user] == password;

            if (isKnownUser)
                IsLoggedIn = true;

            return Task.FromResult(isKnownUser);
        }

        Task IAccountStore.Logout()
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
            KnownUsers.AddOrUpdate(user, s => pass, (s, s1) => throw new Exception("User is already known"));
        }
    }
}