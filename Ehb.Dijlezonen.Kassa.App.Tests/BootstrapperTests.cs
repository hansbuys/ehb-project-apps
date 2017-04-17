using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Tests.Assertions;
using Ehb.Dijlezonen.Kassa.App.Tests.Fakes;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests
{
    public class BootstrapperTests : IoCBasedTest<TestBootstrapper>
    {
        public BootstrapperTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MainPageViewModelGetsRegisteredForMainPage()
        {
            var navigationService = new FakeNavigationService();

            GetSut().RegisterViews(navigationService);

            navigationService.Should().HaveRegistered<MainPage, MainPageViewModel>();
        }
    }
}
