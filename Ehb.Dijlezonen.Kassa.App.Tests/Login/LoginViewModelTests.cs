using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Login
{
    public class LoginViewModelTests : ViewModelTest<LoginViewModel>
    {
        public LoginViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        protected void Login(LoginViewModel vm, string user, string password)
        {
            vm.User = user;
            vm.Password = password;

            vm.LoginCommand.Click();
        }

        protected override bool IsModalWindow => true;

        [Fact]
        public async Task CanLoginWithKnownUserAndPassword()
        {
            await LoginHappyPath();

            AccountStore.Should().BeLoggedIn();
        }

        private async Task LoginHappyPath()
        {
            const string user = "knownUser";
            const string pass = "knownPassword4KnownUser";

            AccountStore.WhenUserIsKnown(user, pass);
            Login(await GetSut(), user, pass);
        }

        [Fact]
        public async Task CannotLoginWithUnknownUserAndPassword()
        {
            const string user = "knownUser";
            const string pass = "knownPassword4KnownUser";

            AccountStore.WhenUserIsKnown(user, pass);
            Login(await GetSut(), "unknownUser", "wrongPassword");

            AccountStore.Should().NotBeLoggedIn();
        }

        [Fact]
        public async Task LoginIsDisabledWithoutUserAndPassword()
        {
            var vm = await GetSut();

            vm.User = string.Empty;
            vm.Password = string.Empty;

            vm.LoginCommand.Should().BeDisabled();
        }

        [Fact]
        public async Task LoginIsEnabledWithUserAndPassword()
        {
            var vm = await GetSut();

            vm.User = "anything";
            vm.Password = "anything";

            vm.LoginCommand.Should().BeEnabled();
        }
    }
}