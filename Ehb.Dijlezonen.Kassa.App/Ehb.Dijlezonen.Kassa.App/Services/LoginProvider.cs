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

        public LoginProvider(IBackendConfiguration config)
        {
            this.config = config;
        }

        public event EventHandler LoggedIn;
        private Token Token { get; set; }

        Token ILoginProvider.Token => Token;

        Task<bool> ILoginProvider.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn());
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
                dynamic token = JsonConvert.DeserializeObject(tokenAsJson);

                if (result.IsSuccessStatusCode)
                {
                    var accessToken = (string)token.access_token;

                    Token = new Token(
                        accessToken, 
                        DateTime.UtcNow.AddSeconds((int) token.expires_in), 
                        IsAdminToken(accessToken));


                    LoggedIn?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private bool IsAdminToken(string token)
        {
            return false;
        }

        Task ILoginProvider.Logout()
        {
            Token = null;

            LoggedOut?.Invoke(null, EventArgs.Empty);

            return Task.FromResult(0);
        }

        public event EventHandler LoggedOut;

        private bool IsLoggedIn()
        {
            return Token.IsValid;
        }
    }
}