using System;
using System.Threading.Tasks;
using Autofac;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public abstract class IoCBasedTest<T> : TestBase, IDisposable
        where T : class
    {
        private readonly IContainer container;
        private readonly ITestOutputHelper output;

        protected IoCBasedTest(ITestOutputHelper output) : base(output)
        {
            this.output = output;
            container = InitializeContainer();
        }

        protected virtual IContainer InitializeContainer()
        {
            return GetBootstrapper().Initialize(builder =>
            {
                builder.RegisterInstance(output);
                builder.RegisterInstance<Logging>(Logging);

                Configure(builder);
            });
        }

        /// <summary>
        /// Override this method to be able to use a different bootstrapper implementation.
        /// </summary>
        /// <returns></returns>
        protected virtual BootstrapperBase GetBootstrapper()
        {
            return new TestBootstrapper(Logging);
        }

        /// <summary>
        /// Use this method to register additional dependencies.
        /// </summary>
        /// <param name="builder">Allows registration of dependencies.</param>
        protected virtual void Configure(ContainerBuilder builder)
        {
        }

        /// <summary>
        /// Use this method to create an instance of your system under test through dependency injection.
        /// </summary>
        /// <returns></returns>
        protected virtual Task<T> GetSut()
        {
            return Task.FromResult(container.Resolve<T>());
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                container?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}