using System.Threading.Tasks;
using Xamarin.Auth;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    //TODO: implement this properly please
    public class XamarinAccountStore : IAccountStore
    {
        private readonly AccountStore accountStore;
        private bool isLoggedIn;

        private const string user = "test";
        private const string password = "test";

        public XamarinAccountStore(AccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        Task<bool> IAccountStore.IsLoggedIn()
        {
            return Task.FromResult(isLoggedIn);
        }

        Task<bool> IAccountStore.Login(string user, string password)
        {
            if (user == XamarinAccountStore.user && password == XamarinAccountStore.password)
            isLoggedIn = true;

            return Task.FromResult(isLoggedIn);
        }
    }
}