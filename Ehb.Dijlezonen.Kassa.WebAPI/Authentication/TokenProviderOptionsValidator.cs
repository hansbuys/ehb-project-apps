using System;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Authentication
{
    public class TokenProviderOptionsValidator
    {
        public Exception GetExceptionWhenInvalid(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
                return new ArgumentNullException(nameof(TokenProviderOptions.Path));

            if (string.IsNullOrEmpty(options.Issuer))
                return new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience))
                return new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero)
                return new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));

            if (options.SigningCredentials == null)
                return new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));

            if (options.NonceGenerator == null)
                return new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));

            return null;
        }
    }
}