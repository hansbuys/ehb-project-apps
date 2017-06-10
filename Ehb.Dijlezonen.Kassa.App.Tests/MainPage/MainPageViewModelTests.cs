using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.MainPage
{
    public class MainPageViewModelTests : ViewModelTest<MainPageViewModel>
    {
        public MainPageViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task WhenNotLoggedInLoginViewGetsDisplayed()
        {
            await GetSut();

            NavigationAdapter.Should().BeDisplaying<LoginViewModel>(true);
        }

        [Fact]
        public async Task ShouldReturnToMainPageAfterLogin()
        {
            const string user = "test";
            const string pass = "test";
            Authentication.WhenUserIsKnown(user, pass);

            await GetSut();

            var loginVm = NavigationAdapter.Should().BeDisplaying<LoginViewModel>(true).Which;
            loginVm.User = user;
            loginVm.Password = pass;
            loginVm.LoginCommand.Click();

            NavigationAdapter.Should().NotBeDisplaying<LoginViewModel>();
            NavigationAdapter.Should().BeDisplaying<MainPageViewModel>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task AdminSectionShouldBeAvailableForAdminAfterLogin(bool userIsAdmin)
        {
            const string user = "test";
            const string pass = "test";
            Authentication.WhenUserIsKnown(user, pass, isAdmin: userIsAdmin);

            var vm = await GetSut();

            var loginVm = NavigationAdapter.Should().BeDisplaying<LoginViewModel>(true).Which;
            loginVm.User = user;
            loginVm.Password = pass;
            loginVm.LoginCommand.Click();

            NavigationAdapter.Should().NotBeDisplaying<LoginViewModel>();
            NavigationAdapter.Should().BeDisplaying<MainPageViewModel>();

            if (userIsAdmin)
                vm.NavigateToAdminCommand.Should().BeEnabled();
            else
                vm.NavigateToAdminCommand.Should().BeDisabled();
        }

        [Fact]
        public async Task LogoutLeadsToLogin()
        {
            Authentication.WhenUserIsLoggedIn();

            (await GetSut()).LogoutCommand.Click();

            Authentication.Should().NotBeLoggedIn();
            NavigationAdapter.Should().BeDisplaying<LoginViewModel>(true);
        }

        [Fact]
        public async Task AdminCanNavigateToAdminOverview()
        {
            Authentication.WhenAdminIsLoggedIn();

            var vm = await GetSut();
            vm.IsAdmin.Should().BeTrue();
            vm.NavigateToAdminCommand.Should().BeEnabled();
            vm.NavigateToAdminCommand.Click();
            
            NavigationAdapter.Should().BeDisplaying<OverviewViewModel>();
        }
    }
}
