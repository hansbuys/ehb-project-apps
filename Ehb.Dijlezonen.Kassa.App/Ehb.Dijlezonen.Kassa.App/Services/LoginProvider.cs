using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class LoginProvider : ILoginProvider
    {
        private readonly IBackendConfiguration config;
        private readonly ILog log;

        public LoginProvider(IBackendConfiguration config, Logging logging)
        {
            this.config = config;
            this.log = logging.GetLoggerFor<LoginProvider>();
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
            using (var httpClient = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage result = null;

                try
                {
                    result = await httpClient.PostAsync(config.BaseUrl + "/token", new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("username", user),
                            new KeyValuePair<string, string>("password", password)
                        }));
                }
                catch (Exception e)
                {
                    log.Error("Unable to make the call", e);
                }

                if (result != null && result.IsSuccessStatusCode)
                {
                    var tokenAsJson = await result.Content.ReadAsStringAsync();
                    dynamic token = JsonConvert.DeserializeObject(tokenAsJson);

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
            return Token != null && Token.IsValid;
        }
    }
}