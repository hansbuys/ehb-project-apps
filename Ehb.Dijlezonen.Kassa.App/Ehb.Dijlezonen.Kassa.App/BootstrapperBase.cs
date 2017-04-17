using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.ResolveAnything;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared
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

        internal IContainer StartContainer(INavigation navigation)
        {
            return IoC.InitializeContainer(builder =>
            {
                builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

                RegisterDependencies(builder);

                builder.RegisterInstance(navigation);
            });
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            RegisterPorts(builder);
            RegisterComponents(builder);
        }

        public virtual void RegisterViews(INavigationService navigation)
        {
            var sharedAssembly = typeof(BootstrapperBase).GetTypeInfo().Assembly;
            RegisterViews(navigation, sharedAssembly);

            var assembly = GetType().GetTypeInfo().Assembly;
            RegisterViews(navigation, assembly);
        }

        private static void RegisterViews(INavigationService navigation, Assembly assembly)
        {
            var views =
                assembly.DefinedTypes.Where(
                    t => !string.IsNullOrEmpty(t.Namespace) && typeof(Page).GetTypeInfo().IsAssignableFrom(t));

            foreach (var view in views)
            {
                var viewModel =
                    assembly.DefinedTypes.SingleOrDefault(
                        t =>
                            !string.IsNullOrEmpty(t.Namespace) && t.Name.StartsWith(view.Name) &&
                            (t.Name.EndsWith("ViewModel") ||
                             t.Name.EndsWith("VM")));

                navigation.Register(view.AsType(), viewModel.AsType());
            }
        }
    }
}