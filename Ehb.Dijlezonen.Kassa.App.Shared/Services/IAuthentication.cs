using System;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IAuthentication
    {
        Task Login(string user, string password);
        event EventHandler LoggedIn;
        event EventHandler NeedsPasswordChange;

        Task Logout();
        event EventHandler LoggedOut;

        User LoggedInUser { get; }

    }
}