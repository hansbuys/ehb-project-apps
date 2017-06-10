using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class Authentication : IAuthentication
    {
        private readonly BackendClient client;
        private readonly Func<JwtSecurityTokenHandler> getTokenHandler;
        private readonly ILog log;

        private User user;
        private Token token;

        private event EventHandler LoggedIn;

        event EventHandler IAuthentication.LoggedIn
        {
            add => LoggedIn += value;
            remove => LoggedIn -= value;
        }

        private event EventHandler LoggedOut;

        event EventHandler IAuthentication.LoggedOut
        {
            add => LoggedOut += value;
            remove => LoggedOut -= value;
        }

        private event EventHandler NeedsPasswordChange;
        event EventHandler IAuthentication.NeedsPasswordChange
        {
            add => NeedsPasswordChange += value;
            remove => NeedsPasswordChange -= value;
        }

        public Authentication(BackendClient client, Func<JwtSecurityTokenHandler> getTokenHandler, Logging logging)
        {
            this.client = client;
            this.getTokenHandler = getTokenHandler;
            this.log = logging.GetLoggerFor<Authentication>();
        }

        async Task IAuthentication.Login(string username, string password)
        {
            var result = await client.PostForm("/token", new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            
            log.Debug($"{username} has succesfully logging in.");

            var tokenAsJson = await result.GetContent;
            dynamic dynamicAccessToken = JsonConvert.DeserializeObject(tokenAsJson);

            var accessToken = (string) dynamicAccessToken.access_token;

            client.AccessToken = accessToken;

            token = new Token(ParseExpirationDateTime(accessToken));

            user = new User(
                ParseIsAdminToken(accessToken),
                ParseNeedsPasswordChange(accessToken));

            OnLoggedIn();

            if (user.NeedsPasswordChange)
            {
                log.Debug($"{user} needs to change passwords.");
                OnNeedsPasswordChange();
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

        Task IAuthentication.Logout()
        {
            user = null;
            token = null;
            client.AccessToken = null;

            OnLoggedOut();

            return Task.FromResult(0);
        }

        User IAuthentication.LoggedInUser
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

        protected virtual void OnLoggedIn()
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLoggedOut()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnNeedsPasswordChange()
        {
            NeedsPasswordChange?.Invoke(this, EventArgs.Empty);
        }
    }
}