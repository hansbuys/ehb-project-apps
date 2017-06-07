using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Newtonsoft.Json;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class LoginProvider : ILoginProvider
    {
        private readonly IBackendConfiguration config;
        private readonly Func<JwtSecurityTokenHandler> getTokenHandler;
        private readonly ILog log;

        public LoginProvider(IBackendConfiguration config, Logging logging, Func<JwtSecurityTokenHandler> getTokenHandler)
        {
            this.config = config;
            this.getTokenHandler = getTokenHandler;
            this.log = logging.GetLoggerFor<LoginProvider>();
        }

        public event EventHandler LoggedIn;
        public event EventHandler LoggedOut;
        public event EventHandler NeedsPasswordChange;
        public event EventHandler PasswordHasChanged;

        private Token Token { get; set; }
        public async Task ChangePassword(string oldPassword, string newPassword)
        {
            var response = await PostJson(config.BaseUrl + "/password/change", new
            {
                oldPassword,
                newPassword
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception("Password change failed!");

            PasswordHasChanged?.Invoke(this, EventArgs.Empty);
        }


        protected async Task<HttpResponseMessage> PostJson(string url, object body)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token.Value);

                var postBody = JsonConvert.SerializeObject(body);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json"));
            }
        }

        Token ILoginProvider.Token => Token;

        Task<bool> ILoginProvider.IsLoggedIn()
        {
            return Task.FromResult(IsLoggedIn());
        }

        async Task ILoginProvider.Login(string user, string password)
        {
            using (var httpClient = new HttpClient())
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
                        ParseIsAdminToken(accessToken),
                        ParseNeedsPasswordChange(accessToken));


                    LoggedIn?.Invoke(null, EventArgs.Empty);

                    if (Token.NeedsPasswordChange)
                        NeedsPasswordChange?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        private bool ParseIsAdminToken(string token)
        {
            return GetClaims(token)
                .Any(x => x.Type == CustomClaimTypes.Role && x.Value == ClaimRoleTypes.Admin);
        }

        private IEnumerable<Claim> GetClaims(string token)
        {
            return getTokenHandler().ReadJwtToken(token).Claims;
        }

        private bool ParseNeedsPasswordChange(string token)
        {
            return GetClaims(token).Any(
                x => x.Type == CustomClaimTypes.NeedsPasswordChange);
        }

        Task ILoginProvider.Logout()
        {
            Token = null;

            LoggedOut?.Invoke(null, EventArgs.Empty);

            return Task.FromResult(0);
        }
        
        private bool IsLoggedIn()
        {
            return Token != null && Token.IsValid;
        }
    }
}