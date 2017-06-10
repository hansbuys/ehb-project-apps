using System.Linq;
using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Ehb.Dijlezonen.Kassa.Infrastructure.Authentication;

namespace Ehb.Dijlezonen.Kassa.App.Tests.Admin
{
    public class AddUserViewModelTests : ViewModelTest<AddUserViewModel>
    {
        public AddUserViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CanRegisterNewUser()
        {
            var vm = await GetSut();

            NavigationAdapter.Should().BeDisplaying<AddUserViewModel>();

            vm.AddNewUserCommand.Should().BeDisabled();

            vm.EmailAddress = "John.Doe@student.ehb.be";
            vm.Firstname = "John";
            vm.Lastname = "Doe";
            vm.Password = "john-doe-password";
            vm.ConfirmPassword = vm.Password;

            vm.AddNewUserCommand.Should().BeEnabled();

            vm.AddNewUserCommand.Click();

            vm.AddNewUserCommand.Should().BeDisabled();

            CredentialService.Should().HaveRegistered("John", "Doe");

            NavigationAdapter.Should().NotBeDisplaying<AddUserViewModel>();
        }

        [Fact]
        public async Task DisplaysAllRoles()
        {
            var vm = await GetSut();

            Duty.AllDuties.ToList().ForEach(r =>
            {
                vm.Roles.Should().Contain(rvm => 
                    rvm.DisplayName == r.DisplayName &&
                    rvm.Name == r.Name);
            });
        }
    }
}
