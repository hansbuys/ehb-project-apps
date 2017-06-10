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
            var username = "John";
            var password = "blank-text-password";

            await LoginUsingAdminCredentials();

            var response = await CreateNewUser(username, password);
            response.EnsureSuccessStatusCode();

            Logout();

            response = await DoLoginUsingCredentials(username, password);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AdminCannotRegisterExistingUser()
        {
            var username = "John";
            var password = "blank-text-password";

            await LoginUsingAdminCredentials();

            var response = await CreateNewUser(username, password);
            response.EnsureSuccessStatusCode();

            response = await CreateNewUser(username, password);
            response.IsSuccessStatusCode.Should().BeFalse();
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