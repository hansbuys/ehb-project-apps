using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;

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
            using (var httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var result = await httpClient.PostAsync("http://localhost:44307/api/token", new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("username", user),
                        new KeyValuePair<string, string>("password", password)
                    }));

                return result.IsSuccessStatusCode;
            }
        }

        public Task Logout()
        {
            isLoggedIn = false;

            return Task.FromResult(0);
        }
    }
}