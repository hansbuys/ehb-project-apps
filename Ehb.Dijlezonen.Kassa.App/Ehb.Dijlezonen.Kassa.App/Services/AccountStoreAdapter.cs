using System.Threading.Tasks;
using Xamarin.Auth;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    //TODO: implement this properly please
    public class AccountStoreAdapter : IAccountStore
    {
        private readonly AccountStore accountStore;
        private bool isLoggedIn;

        private const string TestUser = "test";
        private const string TestPassword = "test";

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
            if (user == TestUser && password == TestPassword)
                isLoggedIn = true;

            return Task.FromResult(isLoggedIn);
        }

        public Task Logout()
        {
            isLoggedIn = false;

            return Task.FromResult(0);
        }
    }
}