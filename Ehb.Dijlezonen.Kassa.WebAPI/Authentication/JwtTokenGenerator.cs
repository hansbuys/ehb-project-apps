using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class JwtTokenGenerator
    {
        private readonly TokenProviderOptions options;
        private readonly ILogger<TokenProviderMiddleware> log;

        public JwtTokenGenerator(IOptions<TokenProviderOptions> options, ILogger<TokenProviderMiddleware> log)
        {
            this.options = options.Value;
            this.log = log;
        }

        public async Task<object> GenerateToken(string username, string password, IIdentityRepository identityRepository)
        {
            log.LogDebug($"attempting to log in user {username}.");

            var identity = await identityRepository.GetIdentity(username, password);
            if (identity == null)
            {
                log.LogError($"{username} logged in with incorrect password.");
                return null;
            }

            log.LogDebug($"successfully logged in user {username}.");
            log.LogDebug($"returning token for {username} which will expire in {options.Expiration.TotalSeconds} seconds.");

            return new
            {
                access_token = await CreateJwtToken(identity)
            };
        }

        private async Task<string> CreateJwtToken(Identity identity)
        {
            var now = DateTime.UtcNow;

            //???
            var claim = new ClaimsIdentity(new GenericIdentity(identity.Name, "Token"), new Claim[] { });

            var claims = new List<Claim>
            {
                SubjectClaim(identity),
                await NonceClaim(),
                IssuedTimestampClaim(now),
                NameClaim(identity)
            };

            var notBefore = now.AddSeconds(-1);
            var tokenExpiration = now.Add(options.Expiration);

            var payload = new JwtPayload(options.Issuer, options.Audience, claims, notBefore, tokenExpiration);

            identity.Roles.ToList().ForEach(r =>
                payload.AddClaim(new Claim(ClaimTypes.Role, r))
            );

            if (identity.NeedsPasswordChange)
                payload.AddClaim(new Claim(CustomClaimTypes.NeedsPasswordChange, "true"));

            var jwtToken = new JwtSecurityToken(new JwtHeader(options.SigningCredentials), payload);
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            return jwtTokenHandler.WriteToken(jwtToken);
        }

        private static Claim NameClaim(Identity identity)
        {
            return new Claim(ClaimTypes.Name, identity.Name);
        }

        private static Claim SubjectClaim(Identity identity)
        {
            return new Claim(JwtRegisteredClaimNames.Sub, identity.Name);
        }

        private async Task<Claim> NonceClaim()
        {
            return new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator());
        }

        private static Claim IssuedTimestampClaim(DateTime now)
        {
            return new Claim(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64);
        }
    }
}