using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.PasswordChange
{
    public class PasswordChangeViewModelTests : ViewModelTest<PasswordChangeViewModel>
    {
        protected FakeCredentialService CredentialService { get; } = new FakeCredentialService();

        public PasswordChangeViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override void Configure(ContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(CredentialService).As<ICredentialService>();
        }

        [Fact]
        public async Task CanChangePassword()
        {
            var vm = await GetSut();

            CredentialService.PasswordChanged.Should().BeFalse();

            vm.OldPassword = "pass";
            vm.NewPassword = "new-pass";
            vm.ConfirmNewPassword = "new-pass";
            vm.ChangePasswordCommand.Click();

            CredentialService.PasswordChanged.Should().BeTrue();
        }

        [Fact]
        public async Task ChangePasswordCommandGetEnabled()
        {
            var vm = await GetSut();

            vm.ChangePasswordCommand.Should().BeDisabled();

            vm.OldPassword = "oud-paswoord";
            vm.NewPassword = "nieuw-paswoord";
            vm.ConfirmNewPassword = "nieuw-paswoord";

            vm.ChangePasswordCommand.Should().BeEnabled();
        }

        [Fact]
        public async Task ChangePasswordCommandStaysDisabledWhenWrongConfirmedPassword()
        {
            var vm = await GetSut();

            vm.OldPassword = "oud-paswoord";
            vm.NewPassword = "nieuw-paswoord";
            vm.ConfirmNewPassword = "verkeerd-nieuw-paswoord";

            vm.ChangePasswordCommand.Should().BeDisabled();
        }
    }
}
