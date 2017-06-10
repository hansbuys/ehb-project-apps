using System;
using System.Threading.Tasks;
using Common.Logging.Configuration;
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
            await RunHappyPath();
        }

        [Fact]
        public async Task AdminCannotRegisterSameUserTwice()
        {
            await RunHappyPath();
            await RunHappyPath(o =>
            {
                o.RegistrationShouldSucceed = false;
                o.RegistrationShouldFail = true;
            });
        }

        [Fact]
        public async Task UserCannotRegisterNewUser()
        {
            await RunHappyPath(o =>
            {
                o.CreateUserUsingAdminLogin = false;
                o.CreateUserUsingUserLogin = true;

                o.RegistrationShouldSucceed = false;
                o.RegistrationShouldFail = true;
            });
        }

        [Fact]
        public async Task NotLoggedInCannotRegisterNewUser()
        {
            await RunHappyPath(o =>
            {
                o.CreateUserUsingAdminLogin = false;
                o.CreateUserUsingUserLogin = false;

                o.RegistrationShouldSucceed = false;
                o.RegistrationShouldFail = true;
            });
        }

        [Fact]
        public async Task UserRegistrationAddsCorrectFieldsToDatabase()
        {
            var username = "";
            var firstname = "";
            var lastname = "";
            await RunHappyPath(o =>
            {
                username = o.Username;
                firstname = o.Firstname;
                lastname = o.Lastname;
            });

            Context.Users.Should().ContainSingle(x => 
                x.Username == username &&
                x.AskNewPasswordOnNextLogin &&
                x.Firstname == firstname && 
                x.Lastname == lastname);
        }

        private async Task RunHappyPath(Action<HappyPathOptions> setup = null)
        {
            var options = new HappyPathOptions();
            setup?.Invoke(options);

            if (options.CreateUserUsingAdminLogin)
                await LoginUsingAdminCredentials();
            else if (options.CreateUserUsingUserLogin)
                await LoginUsingUserCredentials();

            var response = await CreateNewUser(options.Username,
                options.Password,
                options.PasswordNeedsResetOnNextLogin,
                options.Firstname,
                options.Lastname);

            if (options.RegistrationShouldSucceed)
            {
                response.EnsureSuccessStatusCode();
            }
            else if (options.RegistrationShouldFail)
            {
                response.IsSuccessStatusCode.Should().BeFalse();
                return;
            }

            Logout();

            response = await DoLoginUsingCredentials(options.LoginUsername, options.LoginPassword);

            if (options.LoginAfterRegistrationShouldSucceed)
                response.EnsureSuccessStatusCode();
            else if (options.LoginAfterRegistrationShouldFail)
                response.IsSuccessStatusCode.Should().BeFalse();
        }

        private class HappyPathOptions
        {
            public string Username { get; set; } = "john.doe-reborn@sub.domain-name.tld";
            public string Password { get; set; } = "will-be-encrypted";
            public string Firstname { get; set; } = "John";
            public string Lastname { get; set; } = "Doe";
            public bool PasswordNeedsResetOnNextLogin { get; set; } = true;
            public bool IsBlocked { get; set; }

            public bool CreateUserUsingAdminLogin { get; set; } = true;
            public bool CreateUserUsingUserLogin { get; set; } = false;

            public bool RegistrationShouldSucceed { get; set; } = true;
            public bool RegistrationShouldFail { get; set; } = false;

            public bool LoginAfterRegistrationShouldSucceed { get; set; } = true;
            public bool LoginAfterRegistrationShouldFail { get; set; } = false;

            public string LoginUsername { get; set; } = "john.doe-reborn@sub.domain-name.tld";
            public string LoginPassword { get; set; } = "will-be-encrypted";
        }
    }
}