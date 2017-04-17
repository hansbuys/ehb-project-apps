using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests
{
    public class MainPageViewModelTests : MainPageViewModelTestsBase
    {
        public MainPageViewModelTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanNavigateToSecondStage()
        {
            ClickNavigateToSecondStageButton();

            Navigation.Should().HaveNavigatedTo<SecondStageViewModel>();
        }
    }
}
