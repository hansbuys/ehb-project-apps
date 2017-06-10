using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;

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

        [Fact]
        public void DifferentPasswordsGetEncryptedAndVerificationFails()
        {
            var crypto = new Crypto();

            var secure = crypto.Encrypt("password");
            crypto.Verify(secure, "other-password").Should().BeFalse();
        }

        [Fact]
        public void NoTwoPasswordsAreIdentical()
        {
            var crypto = new Crypto();

            var first = crypto.Encrypt("password");
            var second = crypto.Encrypt("password");

            first.Should().NotBe(second);
        }
    }
}
