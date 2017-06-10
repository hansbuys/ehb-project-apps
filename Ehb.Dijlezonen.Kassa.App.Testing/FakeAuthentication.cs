using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeAuthentication : IAuthentication
    {
        private TestUser loggedInUser;
        public ConcurrentBag<TestUser> TestUsers { get; } = new ConcurrentBag<TestUser>();

        public class TestUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool NeedsPasswordChange { get; set; }
            public bool IsAdmin { get; set; }
        }

        private event EventHandler NeedsPasswordChange;

        event EventHandler IAuthentication.NeedsPasswordChange
        {
            add => NeedsPasswordChange += value;
            remove => NeedsPasswordChange -= value;
        }

        Task IAuthentication.Logout()
        {
            loggedInUser = null;
            OnLoggedOut();
            return Task.FromResult(0);
        }

        private event EventHandler LoggedOut;

        event EventHandler IAuthentication.LoggedOut
        {
            add => LoggedOut += value;
            remove => LoggedOut -= value;
        }

        Task IAuthentication.Login(string user, string password)
        {
            loggedInUser = TestUsers.SingleOrDefault(x => x.Username == user && x.Password == password);

            if (loggedInUser != null)
            {
                OnLoggedIn();

                if (loggedInUser.NeedsPasswordChange)
                    OnNeedsPasswordChange();
            }

            return Task.FromResult(0);
        }

        private event EventHandler LoggedIn;

        event EventHandler IAuthentication.LoggedIn
        {
            add => LoggedIn += value;
            remove => LoggedIn -= value;
        }

        public User LoggedInUser => loggedInUser != null ? new User(loggedInUser.IsAdmin, loggedInUser.NeedsPasswordChange) : null;

        public void WhenUserIsLoggedIn(string password = null)
        {
            loggedInUser = new TestUser
            {
                IsAdmin = false,
                Password = password
            };
        }

        public void WhenUserIsKnown(string user, string pass, bool userNeedsPasswordChange = false, bool isAdmin = false)
        {
            TestUsers.Add(new TestUser
            {
                Username = user,
                Password = pass,
                NeedsPasswordChange = userNeedsPasswordChange,
                IsAdmin = isAdmin
            });
        }

        public void WhenAdminIsLoggedIn()
        {
            loggedInUser = new TestUser
            {
                IsAdmin = true
            };
        }

        protected virtual void OnLoggedIn()
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoggedOut()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNeedsPasswordChange()
        {
            NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
        }
    }
}