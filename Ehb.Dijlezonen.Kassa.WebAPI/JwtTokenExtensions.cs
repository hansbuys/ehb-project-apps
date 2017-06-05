using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public static class JwtTokenExtensions
    {
        public static void SetupJwtBearerAuth(this IApplicationBuilder app, IConfigurationSection configuration, IIdentityResolver identityResolver)
        {
            var signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration.GetSection("SecretKey").Value));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("Issuer").Value,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Audience").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                RequireHttpsMetadata = true
            });

            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = configuration.GetSection("TokenPath").Value,
                Audience = configuration.GetSection("Audience").Value,
                Issuer = configuration.GetSection("Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = identityResolver.GetIdentity
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));
        }
    }

    public interface IIdentityResolver
    {
        Task<ClaimsIdentity> GetIdentity(string username, string password);
    }
}