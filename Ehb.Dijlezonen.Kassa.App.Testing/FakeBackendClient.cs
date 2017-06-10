using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeBackendClient : IBackendClient
    {
        private TestUser loggedInUser;
        public bool IsLoggedIn { get; set; }
        public ConcurrentBag<TestUser> TestUsers { get; } = new ConcurrentBag<TestUser>();

        public class TestUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool NeedsPasswordChange { get; set; }
            public bool IsAdmin { get; set; }
        }

        public Task Logout()
        {
            IsLoggedIn = false;
            return Task.FromResult(0);
        }

        public Task Login(string user, string password)
        {
            loggedInUser = TestUsers.SingleOrDefault(x => x.Username == user && x.Password == password);

            if (loggedInUser != null)
            {
                WhenUserIsLoggedIn();
            }

            return Task.FromResult(0);
        }

        public User LoggedInUser => loggedInUser != null ? new User(loggedInUser.IsAdmin, loggedInUser.NeedsPasswordChange) : null;
        public Task ChangePassword(string oldPassword, string newPassword)
        {
            PasswordChanged = true;

            return Task.FromResult(0);
        }

        public bool PasswordChanged { get; set; }

        public void WhenUserIsLoggedIn()
        {
            IsLoggedIn = true;
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
    }
}