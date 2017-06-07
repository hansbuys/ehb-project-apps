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
        protected FakeLoginProvider LoginProvider { get; } = new FakeLoginProvider();

        private Navigation navigation;

        protected virtual bool IsModalWindow => false;

        protected override IContainer InitializeContainer()
        {
            var container = base.InitializeContainer();

            NavigationAdapter = container.Resolve<INavigationAdapter>() as FakeNavigationAdapter;
            if (NavigationAdapter == null)
                throw new Exception("No navigation adapter implemented!");
            NavigationAdapter.SetResolver(container);

            navigation = container.Resolve<Navigation>();

            return container;
        }

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();
            builder.RegisterInstance(LoginProvider).As<ILoginProvider>();
        }

        protected override Task<TViewModel> GetSut()
        {
            return IsModalWindow ?
                navigation.NavigateToModal<TViewModel>() :
                navigation.NavigateTo<TViewModel>();
        }
    }
}