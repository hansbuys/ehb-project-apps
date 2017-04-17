using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.App.Tests.Fakes;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests
{
    public class MainPageViewModelTestsBase : IoCBasedTest<MainPageViewModel>
    {
        protected FakeNavigationService Navigation { get; }

        protected MainPageViewModelTestsBase(ITestOutputHelper output) : base(output)
        {
            Navigation = new FakeNavigationService();
        }

        protected override void Configure(ContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(Navigation).As<INavigationService>();
        }

        protected void ClickNavigateToSecondStageButton()
        {
            var command = GetSut().NavigateToSecondStageCommand;
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }
}