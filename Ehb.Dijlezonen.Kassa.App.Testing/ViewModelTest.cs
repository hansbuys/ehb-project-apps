using System;
using System.Threading.Tasks;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
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
        protected FakeAuthentication Authentication { get; } = new FakeAuthentication();

        private Navigation navigation;

        protected virtual bool IsModalWindow => false;

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();

            builder.RegisterInstance(Authentication).As<IAuthentication>();
            builder.RegisterInstance(Authentication).As<ICredentialService>();
        }

        protected override BootstrapperBase GetBootstrapper()
        {
            return new TestBootstrapper(Logging);
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