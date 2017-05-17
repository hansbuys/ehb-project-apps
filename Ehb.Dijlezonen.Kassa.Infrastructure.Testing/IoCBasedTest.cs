using System;
using Autofac;
using Autofac.Features.ResolveAnything;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public abstract class IoCBasedTest<T> : TestBase, IDisposable
    {
        private readonly Lazy<IContainer> container;
        private readonly ITestOutputHelper output;

        protected IoCBasedTest(ITestOutputHelper output) : base(output)
        {
            this.output = output;
            container = new Lazy<IContainer>(InitializeContainer);
        }

        protected virtual IContainer InitializeContainer()
        {
            var c = new ContainerBuilder();

            Configure(c);

            return c.Build();
        }

        /// <summary>
        /// Use this method to register additional dependencies.
        /// </summary>
        /// <param name="builder">Allows registration of dependencies.</param>
        protected virtual void Configure(ContainerBuilder builder)
        {
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            builder.RegisterInstance(output);
            builder.RegisterInstance<Logging>(Logging);
        }

        /// <summary>
        /// Use this method to create an instance of your system under test through dependency injection.
        /// </summary>
        /// <returns></returns>
        protected virtual T GetSut()
        {
            return container.Value.Resolve<T>();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing && container.IsValueCreated)
            {
                container.Value.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}