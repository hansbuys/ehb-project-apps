using System.Threading.Tasks;
using Xamarin.Auth;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    //TODO: implement this properly please
    public class AccountStoreAdapter : IAccountStore
    {
        private readonly AccountStore accountStore;
        private bool isLoggedIn;

        private const string user = "test";
        private const string password = "test";

        public AccountStoreAdapter(AccountStore accountStore)
        {
            this.accountStore = accountStore;
        }

        Task<bool> IAccountStore.IsLoggedIn()
        {
            return Task.FromResult(isLoggedIn);
        }

        Task<bool> IAccountStore.Login(string user, string password)
        {
            if (user == AccountStoreAdapter.user && password == AccountStoreAdapter.password)
            isLoggedIn = true;

            return Task.FromResult(isLoggedIn);
        }
    }
}