using System;
using Ehb.Dijlezonen.Kassa.App.Shared;
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

            Navigation.Should().HaveNavigatedToModal<Login, LoginViewModel>();
        }

        [Fact]
        public void CanNavigateToSecondStageWhenLoggedIn()
        {
            WhenLoggedIn();

            GetSut().NavigateToSecondStageCommand.Click();

            Navigation.Should().HaveNavigatedTo<SecondStage, SecondStageViewModel>();
        }
    }
}
