using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly BackendClient client;
        private readonly Func<JwtSecurityTokenHandler> getTokenHandler;

        private User user;
        private Token token;

        public AuthenticationService(BackendClient client, Func<JwtSecurityTokenHandler> getTokenHandler)
        {
            this.client = client;
            this.getTokenHandler = getTokenHandler;
        }

        async Task IAuthenticationService.Login(string username, string password)
        {
            var result = await client.PostForm("/token", new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            if (result != null && result.IsSuccessStatusCode)
            {
                var tokenAsJson = await result.Content.ReadAsStringAsync();
                dynamic dynamicAccessToken = JsonConvert.DeserializeObject(tokenAsJson);

                var accessToken = (string)dynamicAccessToken.access_token;

                client.AccessToken = accessToken;

                token = new Token(ParseExpirationDateTime(accessToken));

                user = new User(
                    ParseIsAdminToken(accessToken),
                    ParseNeedsPasswordChange(accessToken));
            }
        }

        private DateTime ParseExpirationDateTime(string accessToken)
        {
            return ReadJwtToken(accessToken).ValidTo;
        }

        private bool ParseIsAdminToken(string accessToken)
        {
            return GetClaims(accessToken).Any(x => x.Type == ClaimTypes.Role && x.Value == ClaimRoleTypes.Admin);
        }

        private bool ParseNeedsPasswordChange(string accessToken)
        {
            return GetClaims(accessToken).Any(x => x.Type == CustomClaimTypes.NeedsPasswordChange);
        }

        private IEnumerable<Claim> GetClaims(string accessToken)
        {
            return ReadJwtToken(accessToken).Claims;
        }

        private JwtSecurityToken ReadJwtToken(string accessToken)
        {
            return getTokenHandler().ReadJwtToken(accessToken);
        }

        Task IAuthenticationService.Logout()
        {
            user = null;
            token = null;
            client.AccessToken = null;
            return Task.FromResult(0);
        }

        User IAuthenticationService.LoggedInUser
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

        private class Token
        {
            public Token(DateTime expiration)
            {
                Expiration = expiration;
            }

            private DateTime Expiration { get; }

            public bool IsValid => Expiration > DateTime.UtcNow;
        }
    }
}