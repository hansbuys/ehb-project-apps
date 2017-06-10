using System;
using Autofac;

namespace Ehb.Dijlezonen.Kassa.Infrastructure
{
    public abstract class BootstrapperBase
    {
        /// <summary>
        ///     Register your external dependencies here,
        ///     these should be usable in production code and mocked out in unit tests.
        /// </summary>
        /// <param name="builder">The IoC container builder, use this to register your dependencies.</param>
        protected abstract void RegisterPorts(ContainerBuilder builder);

        /// <summary>
        ///     Register your internal dependencies here,
        ///     these should be usable in both production code and unit tests.
        /// </summary>
        /// <param name="builder">The IoC container builder, use this to register your dependencies.</param>
        protected abstract void RegisterComponents(ContainerBuilder builder);

        public virtual IContainer Initialize(Action<ContainerBuilder> addDependencies = null)
        {
            return IoC.InitializeContainer(builder =>
            {
                RegisterDependencies(builder);

                addDependencies?.Invoke(builder);
            });
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            RegisterPorts(builder);
            RegisterComponents(builder);
        }
    }
}