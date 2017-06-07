using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Authentication.Tests
{
    public class CryptoTests : IoCBasedTest<Crypto>
    {
        public CryptoTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task SamePasswordsGetEncryptedAndVerified()
        {
            var crypto = await GetSut();

            var secure = crypto.Encrypt("password");
            crypto.Verify(secure, "password").Should().BeTrue();
        }

        [Fact]
        public async Task DifferentPasswordsGetEncryptedAndVerificationFails()
        {
            var crypto = await GetSut();

            var secure = crypto.Encrypt("password");
            crypto.Verify(secure, "other-password").Should().BeFalse();
        }

        [Fact]
        public async Task DifferentSaltVerificationFails()
        {
            var crypto = await GetSut();

            var secure = crypto.Encrypt("password");
            
            var other = new SecurePassword(secure.Password, "some-other-salt");
            crypto.Verify(other, "password").Should().BeFalse();
        }

    }
}
