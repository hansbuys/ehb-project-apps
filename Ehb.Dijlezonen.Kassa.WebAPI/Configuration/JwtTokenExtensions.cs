using System;
using System.Text;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Configuration
{
    public static class JwtTokenExtensions
    {
        public static void UseJwtTokenGenerator(this IServiceCollection services, TokenAuthenticationOptions options)
        {
            var signingKey = CreateSymmetricSecurityKey(options);

            services.Configure<TokenProviderOptions>(x =>
            {
                x.Path = options.TokenPath;
                x.Audience = options.Audience;
                x.Issuer = options.Issuer;
                x.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public static void SetupJwtTokenGenerator(this IApplicationBuilder app, TokenAuthenticationOptions options)
        {
            var signingKey = CreateSymmetricSecurityKey(options);

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
        }

        private static SymmetricSecurityKey CreateSymmetricSecurityKey(TokenAuthenticationOptions options)
        {
            var signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(options.SecretKey));
            return signingKey;
        }
    }
}