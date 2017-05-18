using Ehb.Dijlezonen.Kassa.App.Shared.Model;
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
        public void WhenNotLoggedInLoginViewGetsDisplayed()
        {
            GetSut();

            Navigator.Should().HaveNavigatedModallyTo<LoginViewModel>();
        }

        [Fact]
        public void ShouldReturnToMainPageAfterLogin()
        {
            const string user = "test";
            const string pass = "test";
            AccountStore.WhenUserIsKnown(user, pass);
                
            GetSut();
            
            var loginVm = Navigator.Should().HaveNavigatedModallyTo<LoginViewModel>().Which;
            loginVm.User = user;
            loginVm.Password = pass;
            loginVm.LoginCommand.Click();

            Navigator.Should().NotHaveModal<LoginViewModel>();
        }

        [Fact]
        public void LogoutLeadsToLogin()
        {
            AccountStore.WhenUserIsLoggedIn();

            GetSut().LogoutCommand.Click();

            AccountStore.Should().NotBeLoggedIn();
            Navigator.Should().HaveNavigatedModallyTo<LoginViewModel>();
        }

        [Fact]
        public void NavigatesToBarcodeScanner()
        {
            AccountStore.WhenUserIsLoggedIn();

            GetSut().NavigateToBarcodeScannerCommand.Click();
            
            Navigator.Should().HaveNavigatedTo<BarcodeScannerViewModel>();
        }
    }
}
