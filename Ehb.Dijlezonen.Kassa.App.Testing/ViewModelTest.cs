using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public abstract class ViewModelTest<T> : IoCBasedTest<T>
    {
        private TestBootstrapper bootstrapper;
        protected FakeNavigationAdapter Navigator { get; private set; }
        protected FakeAccountStore AccountStore { get; } = new FakeAccountStore();

        protected ViewModelTest(ITestOutputHelper output) : base(output)
        {
            bootstrapper = new TestBootstrapper(Logging);
        }

        protected void WhenUserIsKnown(string user, string pass)
        {
            Options.AccountStoreOptions.KnownUsers.Add(user, pass);
        }

        protected TestOptions Options = new TestOptions();
        protected class TestOptions
        {
            internal FakeAccountStore.TestOptions AccountStoreOptions { get; } = new FakeAccountStore.TestOptions();
        }

        protected void WhenLoggedIn()
        {
            Options.AccountStoreOptions.IsLoggedIn = true;
        }

        protected override T GetSut()
        {
            AccountStore.Initialize(Options.AccountStoreOptions);

            return base.GetSut();
        }

        protected override IContainer InitializeContainer()
        {
            var container = bootstrapper.Initialize(builder =>
            {
                builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();
                builder.RegisterInstance(AccountStore).As<IAccountStore>();
            });

            Navigator = container.Resolve<INavigationAdapter>() as FakeNavigationAdapter;

            return container;
        }
    }
}
