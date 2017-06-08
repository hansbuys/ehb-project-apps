using System;
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
        private readonly JwtTokenGenerator tokenGenerator;
        private readonly TokenProviderOptions options;
        private readonly JsonSerializerSettings serializerSettings;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            ILogger<TokenProviderMiddleware> log,
            JwtTokenGenerator tokenGenerator,
            TokenProviderOptionsValidator validator)
        {
            this.next = next;
            this.log = log;
            this.tokenGenerator = tokenGenerator;

            this.options = options.Value;

            var exception = validator.GetExceptionWhenInvalid(this.options);
            if (exception != null)
                throw exception;

            serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public async Task Invoke(HttpContext context, IIdentityRepository identityRepository)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(options.Path, StringComparison.Ordinal))
            {
                await next(context);
                return;
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
                || !context.Request.HasFormContentType)
            {
                log.LogError($"invalid call to {options.Path}.");
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Bad request.");
                return;
            }
            
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            log.LogDebug("generating token for request.");
            var token = await tokenGenerator.GenerateToken(username, password, identityRepository);

            if (token == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
            }

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(token, serializerSettings));
        }
    }
}