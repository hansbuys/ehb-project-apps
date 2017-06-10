using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using Xunit;
using Xunit.Abstractions;

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

            vm.AddNewUserCommand.Should().BeEnabled();

            vm.AddNewUserCommand.Click();

            vm.AddNewUserCommand.Should().BeDisabled();

            CredentialService.Should().HaveRegistered("John", "Doe");

            NavigationAdapter.Should().NotBeDisplaying<AddUserViewModel>();
        }
    }
}
