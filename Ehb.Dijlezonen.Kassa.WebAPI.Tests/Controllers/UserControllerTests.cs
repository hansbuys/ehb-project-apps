using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Tests.Controllers
{
    public class UserControllerTests : IntegratedTests
    {
        public UserControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AdminCanRegisterNewUser()
        {
            await LoginUsingAdminCredentials();

            var response = await CreateNewUser("John", "blank-text-password");
            response.EnsureSuccessStatusCode();

            Logout();

            response = await DoLoginUsingCredentials("John", "blank-text-password");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UserCannotRegisterNewUser()
        {
            await LoginUsingUserCredentials();

            var response = await CreateNewUser("John", "blank-text-password");
            response.IsSuccessStatusCode.Should().BeFalse();

            Logout();

            response = await DoLoginUsingCredentials("John", "blank-text-password");
            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task NotLoggedInCannotRegisterNewUser()
        {
            var response = await CreateNewUser("John", "blank-text-password");
            response.IsSuccessStatusCode.Should().BeFalse();

            Logout();

            response = await DoLoginUsingCredentials("John", "blank-text-password");
            response.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}