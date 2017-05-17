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
        public void CanLoginWithKnownUserAndPassword()
        {
            const string user = "knownUser";
            const string pass = "knownPassword4KnownUser";

            AccountStore.WhenUserIsKnown(user, pass);
            Login(GetSut(), user, pass);

            AccountStore.Should().BeLoggedIn();
        }

        [Fact]
        public void CannotLoginWithUnknownUserAndPassword()
        {
            const string user = "knownUser";
            const string pass = "knownPassword4KnownUser";

            AccountStore.WhenUserIsKnown(user, pass);
            Login(GetSut(), "unknownUser", "wrongPassword");

            AccountStore.Should().NotBeLoggedIn();
        }

        [Fact]
        public void LoginIsDisabledWithoutUserAndPassword()
        {
            var vm = GetSut();

            vm.User = string.Empty;
            vm.Password = string.Empty;

            vm.LoginCommand.Should().BeDisabled();
        }

        [Fact]
        public void LoginIsEnabledWithUserAndPassword()
        {
            var vm = GetSut();

            vm.User = "anything";
            vm.Password = "anything";

            vm.LoginCommand.Should().BeEnabled();
        }
    }
}