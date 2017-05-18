using System;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public abstract class ViewModelTest<T> : IoCBasedTest<T>
    {
        private readonly TestBootstrapper bootstrapper;

        protected ViewModelTest(ITestOutputHelper output) : base(output)
        {
            bootstrapper = new TestBootstrapper(Logging);
        }

        protected FakeNavigationAdapter Navigator { get; private set; }
        protected FakeAccountStore AccountStore { get; } = new FakeAccountStore();

        protected virtual bool IsModalWindow => false;

        protected override IContainer InitializeContainer()
        {
            var container = bootstrapper.Initialize(builder =>
            {
                builder.RegisterType<FakeNavigationAdapter>().As<INavigationAdapter>().SingleInstance();
                builder.RegisterInstance(AccountStore).As<IAccountStore>();
            });

            Navigator = container.Resolve<INavigationAdapter>() as FakeNavigationAdapter;
            Navigator?.SetResolver(container);

            return container;
        }

        protected override T GetSut()
        {
            var vm = base.GetSut();

            if (IsModalWindow)
                Navigator.ModalStack.AddOrUpdate(typeof(T), t => vm,
                    (t, v) => throw new Exception("Already added to modal stack"));
            else
                Navigator.NavigationStack.AddOrUpdate(typeof(T), t => vm,
                    (t, v) => throw new Exception("Already added to stack"));

            return vm;
        }
    }
}