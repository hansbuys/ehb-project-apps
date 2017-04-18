using System;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using System.Collections.Generic;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeAccountStore : IAccountStore
    {
        Task<bool> IAccountStore.IsLoggedIn()
        {
            return Task.FromResult(Options.IsLoggedIn);
        }

        Task<bool> IAccountStore.Login(string user, string password)
        {
            Dictionary<string, string> knownUsers = Options.KnownUsers;

            bool isKnownUser = knownUsers.ContainsKey(user) && knownUsers[user] == password;

            if (isKnownUser)
                Options.IsLoggedIn = true;

            return Task.FromResult(isKnownUser);
        }

        internal void Initialize(TestOptions options)
        {
            Options = options;
        }

        internal TestOptions Options { get; private set; } = new TestOptions();
        internal class TestOptions
        {
            public bool IsLoggedIn { get; set; }
            public Dictionary<string, string> KnownUsers { get; } = new Dictionary<string, string>();
        }
    }
}