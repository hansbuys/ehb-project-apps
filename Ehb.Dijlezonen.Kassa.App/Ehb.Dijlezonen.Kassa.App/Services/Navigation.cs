using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class Navigation : IDisposable
    {
        private readonly INavigationAdapter navigationAdapter;
        private readonly ILoginProvider login;
        private readonly EventHandler onLoggedIn;
        private readonly EventHandler onLoggedOut;
        private readonly EventHandler needsPasswordChange;

        public Navigation(INavigationAdapter navigationAdapter, ILoginProvider login)
        {
            this.navigationAdapter = navigationAdapter;
            this.login = login;

            onLoggedIn = async (sender, args) => await OnLoggedIn().ConfigureAwait(false);
            onLoggedOut = async (sender, args) => await OnLoggedOut().ConfigureAwait(false);
            needsPasswordChange = async (sender, args) => await NeedsPasswordChange().ConfigureAwait(false);

            login.LoggedIn += onLoggedIn;
            login.LoggedOut += onLoggedOut;
            login.NeedsPasswordChange += needsPasswordChange;
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
            if (!await login.IsLoggedIn() && typeof(TViewModel).IsAssignableTo<IProtectedViewModel>())
                await login.Logout();

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
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue && disposing)
            {
                login.LoggedIn -= onLoggedIn;
                login.LoggedOut -= onLoggedOut;
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