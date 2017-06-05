using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public class Navigation : IDisposable
    {
        private readonly INavigationAdapter navigationAdapter;
        private readonly ILoginProvider login;
        private readonly EventHandler onLoggedOut;

        public Navigation(INavigationAdapter navigationAdapter, ILoginProvider login)
        {
            this.navigationAdapter = navigationAdapter;
            this.login = login;

            onLoggedOut = async (sender, args) => await OnLoggedOut().ConfigureAwait(false);

            login.LoggedOut += onLoggedOut;
        }

        private Task OnLoggedOut()
        {
            return NavigateToLogin();
        }

        public async Task<TViewModel> NavigateTo<TViewModel>()
        {
            if (typeof(TViewModel).IsAssignableTo<IProtectedViewModel>())
            {
                await NavigateToLogin().ConfigureAwait(false);
            }

            return await navigationAdapter.NavigateTo<TViewModel>().ConfigureAwait(false);
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
                login.LoggedIn -= onLoggedOut;
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