using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeLoginProvider : ILoginProvider
    {
        public bool IsLoggedIn { get; set; }
        public ConcurrentBag<TestUser> TestUsers { get; } = new ConcurrentBag<TestUser>();

        public class TestUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool NeedsPasswordChange { get; set; }
        }

        Task<bool> ILoginProvider.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn);
        }

        Task ILoginProvider.Login(string user, string password)
        {
            var testUser = TestUsers.SingleOrDefault(x => x.Username == user && x.Password == password);

            if (testUser != null)
            {
                WhenUserIsLoggedIn();

                if (testUser.NeedsPasswordChange)
                    OnNeedsPasswordChange();
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
        public event EventHandler NeedsPasswordChange;

        public Token Token => null;
        public Task ChangePassword(string newPassword)
        {
            PasswordChanged = true;

            return Task.FromResult(0);
        }

        public bool PasswordChanged { get; set; }

        public void WhenUserIsLoggedIn()
        {
            OnLoggedIn();
            IsLoggedIn = true;
        }

        public void WhenUserIsKnown(string user, string pass, bool optionsNeedsPasswordChange = false)
        {
            TestUsers.Add(new TestUser
            {
                Username = user,
                Password = pass,
                NeedsPasswordChange = optionsNeedsPasswordChange
            });
        }

        protected virtual void OnLoggedOut()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoggedIn()
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNeedsPasswordChange()
        {
            NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
        }
    }
}