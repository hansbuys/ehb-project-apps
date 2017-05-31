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

        protected FakeNavigationAdapter Navigator { get; private set; }
        protected FakeAccountStore AccountStore { get; } = new FakeAccountStore();

        protected virtual bool IsModalWindow => false;

        protected override IContainer InitializeContainer()
        {
            var container = base.InitializeContainer();

            Navigator = container.Resolve<INavigationAdapter>() as FakeNavigationAdapter;
            if (Navigator == null)
                throw new Exception("No navigation adapter implemented!");
            Navigator.SetResolver(container);

            return container;
        }

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();
            builder.RegisterInstance(AccountStore).As<IAccountStore>();
        }

        protected override Task<TViewModel> GetSut()
        {
            var nav = (INavigationAdapter) Navigator;

            return IsModalWindow ? 
                nav.NavigateToModal<TViewModel>() : 
                nav.NavigateTo<TViewModel>();
        }
    }
}