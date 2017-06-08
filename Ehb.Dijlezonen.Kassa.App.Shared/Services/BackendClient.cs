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
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class BackendClient : IBackendClient
    {
        private readonly Func<JwtSecurityTokenHandler> getTokenHandler;
        private readonly IBackendConfiguration config;
        private readonly Func<HttpClient> getHttpClient;
        private readonly ILog log;

        private Token token;
        private User user;

        public BackendClient(Func<JwtSecurityTokenHandler> getTokenHandler, IBackendConfiguration config, Func<HttpClient> getHttpClient, Logging logging)
        {
            this.getTokenHandler = getTokenHandler;
            this.config = config;
            this.getHttpClient = getHttpClient;
            this.log = logging.GetLoggerFor<BackendClient>();
        }

        async Task IBackendClient.Login(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage result = null;

                try
                {
                    result = await httpClient.PostAsync(config.BaseUrl + "/token", new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("username", username),
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
                    dynamic dynamicAccessToken = JsonConvert.DeserializeObject(tokenAsJson);

                    var accessToken = (string)dynamicAccessToken.access_token;

                    this.token = new Token(accessToken, DateTime.UtcNow.AddSeconds((int)dynamicAccessToken.expires_in));

                    this.user = new User(
                        ParseIsAdminToken(accessToken),
                        ParseNeedsPasswordChange(accessToken));
                }
            }
        }

        private bool ParseIsAdminToken(string accessToken)
        {
            return GetClaims(accessToken).Any(x => x.Type == ClaimTypes.Role && x.Value == ClaimRoleTypes.Admin);
        }

        private IEnumerable<Claim> GetClaims(string accessToken)
        {
            return getTokenHandler().ReadJwtToken(accessToken).Claims;
        }

        private bool ParseNeedsPasswordChange(string accessToken)
        {
            return GetClaims(accessToken).Any(x => x.Type == CustomClaimTypes.NeedsPasswordChange);
        }

        Task IBackendClient.Logout()
        {
            user = null;
            token = null;
            return Task.FromResult(0);
        }

        async Task IBackendClient.ChangePassword(string oldPassword, string newPassword)
        {
            var response = await PostJson(config.BaseUrl + "/password/change", new
            {
                oldPassword,
                newPassword
            });

            response.EnsureSuccessStatusCode();
        }

        User IBackendClient.LoggedInUser
        {
            get
            {

                if (token != null && token.IsValid)
                {
                    return user;
                }

                return null;
            }
        }

        private async Task<HttpResponseMessage> PostJson(string url, object body)
        {
            using (var client = GetLoggedInHttpClient())
            {
                var postBody = JsonConvert.SerializeObject(body);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json"));
            }
        }

        private HttpClient GetLoggedInHttpClient()
        {
            if (token == null)
                throw new Exception("You should be logged in first.");

            var client = getHttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Value);

            return client;
        }

        private class Token
        {
            public Token(string value, DateTime expiration)
            {
                Expiration = expiration;
                Value = value;
            }

            private DateTime Expiration { get; }
            public string Value { get; }

            public bool IsValid => !string.IsNullOrEmpty(Value) && Expiration < DateTime.UtcNow;
        }
    }
}