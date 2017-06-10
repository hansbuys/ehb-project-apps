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
        private readonly IAuthentication authentication;
        
        private readonly EventHandler onLoggedOut;
        private readonly EventHandler needsPasswordChange;

        public Navigation(INavigationAdapter navigationAdapter, IAuthentication authentication)
        {
            this.navigationAdapter = navigationAdapter;
            this.authentication = authentication;
            
            onLoggedOut = async (sender, args) => await OnLoggedOut();
            needsPasswordChange = async (sender, args) => await NeedsPasswordChange();
            
            authentication.NeedsPasswordChange += needsPasswordChange;
            authentication.LoggedOut += onLoggedOut;
        }

        private Task NeedsPasswordChange()
        {
            return navigationAdapter.NavigateToModal<PasswordChangeViewModel>();
        }

        private Task OnLoggedOut()
        {
            return NavigateToLogin();
        }

        public async Task<TViewModel> NavigateTo<TViewModel>()
        {

            CheckAdminPrivileges<TViewModel>();

            var vm = await navigationAdapter.NavigateTo<TViewModel>();

            if (RequiresAuthentication<TViewModel>())
                await NavigateToLogin();

            return vm;
        }

        private bool RequiresAuthentication<TViewModel>()
        {
            return authentication.LoggedInUser == null && 
                typeof(TViewModel).IsAssignableTo<IRequireAuthentication>();
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
                authentication.LoggedOut -= onLoggedOut;
                authentication.NeedsPasswordChange -= needsPasswordChange;

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