using Autofac;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public abstract class ViewModelTest<T> : IoCBasedTest<T>
    {
        private TestBootstrapper bootstrapper;
        protected FakeNavigation Navigation { get; } = new FakeNavigation();
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
            return bootstrapper.Initialize(Navigation, AccountStore);
        }
    }
}
