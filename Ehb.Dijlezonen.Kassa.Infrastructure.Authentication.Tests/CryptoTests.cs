using FluentAssertions;
using Xunit;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication.Tests
{
    public class CryptoTests
    {
        [Fact]
        public void SamePasswordsGetEncryptedAndVerified()
        {
            var crypto = new Crypto();

            var secure = crypto.Encrypt("password");
            crypto.Verify(secure, "password").Should().BeTrue();
        }
    }
}
