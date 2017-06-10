﻿using System.Collections.Concurrent;
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

        Task IAuthentication.Logout()
        {
            loggedInUser = null;
            return Task.FromResult(0);
        }

        Task IAuthentication.Login(string user, string password)
        {
            loggedInUser = TestUsers.SingleOrDefault(x => x.Username == user && x.Password == password);

            return Task.FromResult(0);
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
    }
}