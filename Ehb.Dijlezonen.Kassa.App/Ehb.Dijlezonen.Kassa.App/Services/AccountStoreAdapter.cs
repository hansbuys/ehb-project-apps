using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    //TODO: implement this properly please
    public class AccountStoreAdapter : IAccountStore
    {
        private bool isLoggedIn;

        Task<bool> IAccountStore.IsLoggedIn()
        {
            return Task.FromResult(isLoggedIn);
        }

        async Task<bool> IAccountStore.Login(string user, string password)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.PostAsync("https://localhost:44308/api/token", new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("username", user), 
                new KeyValuePair<string, string>("password", password) 
            }));

            return result.IsSuccessStatusCode;
        }

        public Task Logout()
        {
            isLoggedIn = false;

            return Task.FromResult(0);
        }
    }
}