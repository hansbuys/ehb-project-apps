using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using Xunit;
using Xunit.Abstractions;
using Ehb.Dijlezonen.Kassa.App.Testing;

namespace Ehb.Dijlezonen.Kassa.App.Tests
{
    public class MainPageViewModelTests : ViewModelTest<MainPageViewModel>
    {
        public MainPageViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenNotLoggedInLoginViewGetsDisplayed()
        {
            GetSut();

            Navigator.Should().HaveNavigatedToModal<LoginViewModel>();
        }

        [Fact]
        public void ShouldReturnToMainPageAfterLogin()
        {
            var user = "test";
            var pass = "test";
            WhenUserIsKnown(user, pass);
                
            GetSut();
            
            var loginVm = Navigator.Should().HaveNavigatedToModal<LoginViewModel>().Which;
            loginVm.User = user;
            loginVm.Password = pass;
            loginVm.LoginCommand.Click();

            Navigator.Should().NotHaveModal<LoginViewModel>();
        }

        [Fact]
        public void CanNavigateToSecondStageWhenLoggedIn()
        {
            WhenLoggedIn();

            GetSut().NavigateToSecondStageCommand.Click();

            Navigator.Should().HaveNavigatedTo<SecondStageViewModel>();
        }
    }
}
