using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class LoginProvider : ILoginProvider
    {
        private readonly IBackendConfiguration config;

        private string Token { get; set; }
        private DateTime TokenExpiration { get; set; }

        public LoginProvider(IBackendConfiguration config)
        {
            this.config = config;
        }

        Task<bool> ILoginProvider.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn());
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Token) && TokenExpiration > DateTime.UtcNow;
        }

        async Task ILoginProvider.Login(string user, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync(config.BaseUrl + "/token", new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("username", user),
                        new KeyValuePair<string, string>("password", password)
                    })).ConfigureAwait(false);

                var tokenAsJson = await result.Content.ReadAsStringAsync();

                dynamic tokenAsObject = JsonConvert.DeserializeObject(tokenAsJson);

                if (result.IsSuccessStatusCode)
                {
                    Token = tokenAsObject.access_token;
                    int tokenExpirationInSeconds = tokenAsObject.expires_in;
                    TokenExpiration = DateTime.UtcNow.AddSeconds(tokenExpirationInSeconds);
                }
            }
        }

        public Task Logout()
        {
            TokenExpiration = DateTime.MinValue;
            Token = "";

            return Task.FromResult(0);
        }
    }
}