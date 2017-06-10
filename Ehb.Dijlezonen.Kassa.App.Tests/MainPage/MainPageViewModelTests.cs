using System.Threading.Tasks;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.Admin;
using Ehb.Dijlezonen.Kassa.App.Shared.Model.UserManagement;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
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

            NavigationAdapter.Should().HaveNavigatedModallyTo<LoginViewModel>();
        }

        [Fact]
        public async Task ShouldReturnToMainPageAfterLogin()
        {
            const string user = "test";
            const string pass = "test";
            AuthenticationService.WhenUserIsKnown(user, pass);

            await GetSut();

            var loginVm = NavigationAdapter.Should().HaveNavigatedModallyTo<LoginViewModel>().Which;
            loginVm.User = user;
            loginVm.Password = pass;
            loginVm.LoginCommand.Click();

            NavigationAdapter.Should().NotHaveModal<LoginViewModel>();
        }

        [Fact]
        public async Task LogoutLeadsToLogin()
        {
            AuthenticationService.WhenUserIsLoggedIn();

            (await GetSut()).LogoutCommand.Click();

            AuthenticationService.Should().NotBeLoggedIn();
            NavigationAdapter.Should().HaveNavigatedModallyTo<LoginViewModel>();
        }

        [Fact]
        public async Task AdminCanNavigateToAdminOverview()
        {
            AuthenticationService.WhenAdminIsLoggedIn();

            (await GetSut()).NavigateToAdminCommand.Click();
            
            NavigationAdapter.Should().HaveNavigatedTo<OverviewViewModel>();
        }
    }
}
