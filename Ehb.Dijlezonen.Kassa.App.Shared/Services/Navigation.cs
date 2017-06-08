using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class Navigation : IDisposable
    {
        private readonly INavigationAdapter navigationAdapter;
        private readonly UserService userService;
        private readonly IBackendClient backendClient;
        private readonly EventHandler onLoggedIn;
        private readonly EventHandler onLoggedOut;
        private readonly EventHandler needsPasswordChange;
        private readonly EventHandler passwordHasChanged;

        public Navigation(INavigationAdapter navigationAdapter, UserService userService, IBackendClient backendClient)
        {
            this.navigationAdapter = navigationAdapter;
            this.userService = userService;
            this.backendClient = backendClient;

            onLoggedIn = async (sender, args) => await OnLoggedIn().ConfigureAwait(false);
            onLoggedOut = async (sender, args) => await OnLoggedOut().ConfigureAwait(false);
            needsPasswordChange = async (sender, args) => await NeedsPasswordChange().ConfigureAwait(false);
            passwordHasChanged = async (sender, args) => await PasswordHasChanged().ConfigureAwait(false);

            userService.LoggedIn += onLoggedIn;
            userService.LoggedOut += onLoggedOut;
            userService.NeedsPasswordChange += needsPasswordChange;
            userService.PasswordHasChanged += passwordHasChanged;
        }

        private Task PasswordHasChanged()
        {
            return navigationAdapter.CloseModal();
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
            if (backendClient.LoggedInUser == null && typeof(TViewModel).IsAssignableTo<IProtectedViewModel>())
                await userService.Logout();

            return await navigationAdapter.NavigateTo<TViewModel>();
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
                userService.PasswordHasChanged -= passwordHasChanged;

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