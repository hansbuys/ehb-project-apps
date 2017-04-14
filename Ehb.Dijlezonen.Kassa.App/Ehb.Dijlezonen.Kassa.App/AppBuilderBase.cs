using Autofac;
using Autofac.Features.ResolveAnything;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public abstract class AppBuilderBase
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

        internal IContainer StartContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            RegisterDependencies(builder);

            return builder.Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            RegisterPorts(builder);
            RegisterComponents(builder);
        }
    }
}