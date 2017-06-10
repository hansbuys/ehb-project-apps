using System;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;
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

            Authentication.Should().BeLoggedIn();
        }

        private class HappyPathOptions
        {
            public string User { get; set; } = "knownUser";
            public string Pass { get; set; } = "knownPassword4KnownUser";
            public bool NeedsPasswordChange { get; set; }
            public bool IsAdmin { get; set; }
        }

        private async Task LoginHappyPath(Action<HappyPathOptions> setup = null, Action<HappyPathOptions> beforeLogin = null)
        {
            var options = new HappyPathOptions();

            setup?.Invoke(options);

            Authentication.WhenUserIsKnown(options.User, options.Pass, options.NeedsPasswordChange, options.IsAdmin);

            beforeLogin?.Invoke(options);

            Login(await GetSut(), options.User, options.Pass);
        }

        [Fact]
        public async Task CannotLoginWithUnknownUserAndPassword()
        {
            await LoginHappyPath(beforeLogin: o =>
            {
                o.User = "Unknown";
                o.Pass = "Unknown";
            });

            Authentication.Should().NotBeLoggedIn();
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

        [Fact]
        public async Task DisplayPasswordChangeWhenPasswordShouldBeChangedAfterLogin()
        {
            await LoginHappyPath(setup: o => o.NeedsPasswordChange = true);

            NavigationAdapter.Should().HaveNavigatedModallyTo<PasswordChangeViewModel>();
        }
    }
}