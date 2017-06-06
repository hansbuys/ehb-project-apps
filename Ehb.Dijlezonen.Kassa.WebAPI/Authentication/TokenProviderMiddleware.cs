using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    // source: https://dev.to/samueleresca/developing-token-authentication-using-aspnet-core
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<TokenProviderMiddleware> log;
        private readonly TokenProviderOptions options;
        private readonly JsonSerializerSettings serializerSettings;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            ILogger<TokenProviderMiddleware> log)
        {
            this.next = next;
            this.log = log;

            this.options = options.Value;
            ThrowIfInvalidOptions(this.options);

            serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(options.Path, StringComparison.Ordinal))
                return next(context);

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
                || !context.Request.HasFormContentType)
            {
                log.LogError($"invalid call to {options.Path}.");
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }
            
            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            log.LogDebug("generating token for request.");

            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            
            log.LogDebug($"attempting to log in user {username}.");

            var identity = await options.IdentityResolver(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;

                log.LogError($"{username} logged in with incorrect password.");

                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                options.Issuer,
                options.Audience,
                claims.Concat(identity.Claims),
                now,
                now.Add(options.Expiration),
                options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int) options.Expiration.TotalSeconds
            };

            log.LogDebug($"returning token for {username} which will expire in {options.Expiration.TotalSeconds} seconds.");

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
        }
        
        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));

            if (string.IsNullOrEmpty(options.Issuer))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));

            if (options.IdentityResolver == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));

            if (options.NonceGenerator == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
        }
    }
}