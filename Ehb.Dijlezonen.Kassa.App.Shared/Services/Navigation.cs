using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class Navigation : IDisposable
    {
        private readonly INavigationAdapter navigationAdapter;
        private readonly UserService userService;
        private readonly IAuthentication authentication;

        private readonly EventHandler onLoggedIn;
        private readonly EventHandler onLoggedOut;
        private readonly EventHandler needsPasswordChange;

        public Navigation(INavigationAdapter navigationAdapter, UserService userService, IAuthentication authentication)
        {
            this.navigationAdapter = navigationAdapter;
            this.userService = userService;
            this.authentication = authentication;

            onLoggedIn = async (sender, args) => await OnLoggedIn();
            onLoggedOut = async (sender, args) => await OnLoggedOut();
            needsPasswordChange = async (sender, args) => await NeedsPasswordChange();

            userService.LoggedIn += onLoggedIn;
            userService.LoggedOut += onLoggedOut;
            userService.NeedsPasswordChange += needsPasswordChange;
        }

        private Task NeedsPasswordChange()
        {
            return navigationAdapter.NavigateToModal<PasswordChangeViewModel>();
        }

        private Task OnLoggedIn()
        {
            return navigationAdapter.CloseModal();
        }

        private Task OnLoggedOut()
        {
            return NavigateToLogin();
        }

        public async Task<TViewModel> NavigateTo<TViewModel>()
        {
            if (RequiresLogin<TViewModel>())
                await userService.Logout();

            CheckAdminPrivileges<TViewModel>();

            return await navigationAdapter.NavigateTo<TViewModel>();
        }

        private bool RequiresLogin<TViewModel>()
        {
            return authentication.LoggedInUser == null && 
                typeof(TViewModel).IsAssignableTo<IRequireLogin>();
        }

        private void CheckAdminPrivileges<TViewModel>()
        {
            if (RequiresAdminPrivileges<TViewModel>())
                throw new Exception("This page required admin privileges.");
        }

        private bool RequiresAdminPrivileges<TViewModel>()
        {
            return authentication.LoggedInUser != null && 
                !authentication.LoggedInUser.IsAdmin &&
                typeof(TViewModel).IsAssignableTo<IRequireAdminPrivileges>();
        }

        public Task<TViewModel> NavigateToModal<TViewModel>()
        {
            return navigationAdapter.NavigateToModal<TViewModel>();
        }

        public Task<LoginViewModel> NavigateToLogin()
        {
            return navigationAdapter.NavigateToModal<LoginViewModel>();
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && disposing)
            {
                userService.LoggedIn -= onLoggedIn;
                userService.LoggedOut -= onLoggedOut;
                userService.NeedsPasswordChange -= needsPasswordChange;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}