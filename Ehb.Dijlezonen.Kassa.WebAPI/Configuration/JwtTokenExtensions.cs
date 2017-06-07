using System;
using System.Text;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiddlewareOptions = Microsoft.Extensions.Options.Options;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Configuration
{
    public static class JwtTokenExtensions
    {
        public static void SetupJwtBearerAuth(this IApplicationBuilder app,
            IOptions<TokenAuthenticationOptions> configuration)
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
            };

            app.UseMiddleware<TokenProviderMiddleware>(MiddlewareOptions.Create(tokenProviderOptions));
        }
    }
}