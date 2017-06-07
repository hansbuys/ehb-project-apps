using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests.Controllers
{
    public class PasswordControllerTests : IntegratedTests
    {
        public PasswordControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanChangePassword()
        {
            await LoginUsingUserCredentials();

            var response = await PostJson("/api/password/change", new
            {
                oldPassword = "gebruiker",
                newPassword = "my-new-password"
            });
            response.EnsureSuccessStatusCode();

            Logout();
            
            response = await DoLoginUsingUserCredentials();
            response.IsSuccessStatusCode.Should().BeFalse();
            
            response = await DoLoginUsingUserCredentials("my-new-password");
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task CannotChangePasswordWhenNotLoggedIn()
        {
            var response = await PostJson("/api/password/change", new
            {
                oldPassword = "gebruiker",
                newPassword = "my-new-password"
            });

            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task CannotChangePasswordWithoutOldPassword()
        {
            await LoginUsingUserCredentials();

            var response = await PostJson("/api/password/change", new
            {
                oldPassword = "wrong-old-password",
                newPassword = "my-new-password"
            });

            response.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
