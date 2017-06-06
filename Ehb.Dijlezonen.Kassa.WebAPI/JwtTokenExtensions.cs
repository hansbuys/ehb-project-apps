using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public static class JwtTokenExtensions
    {
        public static void SetupJwtBearerAuth(this IApplicationBuilder app, IOptions<TokenAuthenticationOptions> configuration, IIdentityResolver identityResolver)
        {
            var options = configuration.Value;

            var signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(options.SecretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
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
                Path = options.TokenPath,
                Audience = options.Audience,
                Issuer = options.Issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = identityResolver.GetIdentity
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));
        }
    }
}