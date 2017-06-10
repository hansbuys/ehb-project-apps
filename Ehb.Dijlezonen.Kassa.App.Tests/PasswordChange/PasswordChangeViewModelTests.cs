using System;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.PasswordChange
{
    public class PasswordChangeViewModelTests : ViewModelTest<PasswordChangeViewModel>
    {
        public PasswordChangeViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override bool IsModalWindow => true;

        [Fact]
        public async Task CanChangePassword()
        {
            await RunHappyPath(
                o => CredentialService.PasswordChanged.Should().BeFalse());
            
            CredentialService.PasswordChanged.Should().BeTrue();
            NavigationAdapter.Should().NotBeDisplaying<PasswordChangeViewModel>();
        }

        [Fact]
        public async Task WhileChangingPasswordPasswordChangeShouldBeDisabled()
        {
            var vm = await RunHappyPath();

            vm.ChangePasswordCommand.Should().BeDisabled();
        }

        [Fact]
        public async Task ChangePasswordIsAvailableWhenFieldsAreSet()
        {
            await RunHappyPath(
                beforeSetFieldValues: v => v.ChangePasswordCommand.Should().BeDisabled(),
                beforeChangePassword: (o, v) => v.ChangePasswordCommand.Should().BeEnabled());
        }

        [Fact]
        public async Task ChangePasswordCommandStaysDisabledWhenWrongConfirmedPassword()
        {
            var vm = await RunHappyPath(o =>
            {
                o.NewPassword = "nieuw-paswoord";
                o.ConfirmNewPassword = "verkeerd-nieuw-paswoord";
            });

            vm.ChangePasswordCommand.Should().BeDisabled();
        }

        private async Task<PasswordChangeViewModel> RunHappyPath(Action<HappyPathOptions> setup = null,
            Action<PasswordChangeViewModel> beforeSetFieldValues = null,
            Action<HappyPathOptions, PasswordChangeViewModel> beforeChangePassword = null)
        {
            var options = new HappyPathOptions();

            setup?.Invoke(options);

            var vm = await GetSut();

            NavigationAdapter.Should().BeDisplaying<PasswordChangeViewModel>();

            beforeSetFieldValues?.Invoke(vm);

            vm.OldPassword = options.OldPassword;
            vm.NewPassword = options.NewPassword;
            vm.ConfirmNewPassword = options.ConfirmNewPassword;

            beforeChangePassword?.Invoke(options, vm);

            vm.ChangePasswordCommand.Click();

            return vm;
        }

        private class HappyPathOptions
        {
            public string OldPassword { get; set; } = "old-password";
            public string NewPassword { get; set; } = "new-password";
            public string ConfirmNewPassword { get; set; } = "new-password";
        }
    }
}
