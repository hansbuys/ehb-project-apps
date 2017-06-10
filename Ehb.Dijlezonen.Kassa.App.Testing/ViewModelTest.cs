using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public abstract class ViewModelTest<TViewModel> : IoCBasedTest<TViewModel>
        where TViewModel : class
    {
        protected ViewModelTest(ITestOutputHelper output) : base(output)
        {
        }

        protected FakeNavigationAdapter NavigationAdapter { get; private set; }
        protected FakeBackendClient BackendClient { get; } = new FakeBackendClient();

        private Navigation navigation;

        protected virtual bool IsModalWindow => false;

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().SingleInstance();
            builder.RegisterType<Navigation>().SingleInstance();

            builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();
            builder.RegisterInstance(BackendClient).As<IBackendClient>().SingleInstance();
        }

        protected override IContainer InitializeContainer()
        {
            var container = base.InitializeContainer();

            NavigationAdapter = container.Resolve<INavigationAdapter>() as FakeNavigationAdapter;
            if (NavigationAdapter == null)
                throw new Exception("No navigation adapter implemented!");

            navigation = container.Resolve<Navigation>();

            return container;
        }

        protected override Task<TViewModel> GetSut()
        {
            return IsModalWindow ?
                navigation.NavigateToModal<TViewModel>() :
                navigation.NavigateTo<TViewModel>();
        }
    }
}