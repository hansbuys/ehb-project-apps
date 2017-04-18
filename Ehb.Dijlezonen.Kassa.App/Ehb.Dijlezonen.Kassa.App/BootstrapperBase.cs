using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.ResolveAnything;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Common.Logging;
using System;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public abstract class BootstrapperBase
    {
        private ILog log;

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
        protected virtual void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().SingleInstance();
        }

        public IContainer Initialize(Action<ContainerBuilder> addDependencies = null)
        {
            IContainer container = IoC.InitializeContainer(builder =>
            {
                RegisterDependencies(builder);

                addDependencies?.Invoke(builder);
            });
            
            var logging = container.Resolve<Logging>();
            log = logging.GetLoggerFor<BootstrapperBase>();

            var navigationService = container.Resolve<ViewFactory>();
            RegisterViews(navigationService);

            return container;
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            RegisterPorts(builder);
            RegisterComponents(builder);
        }

        private void RegisterViews(ViewFactory viewFactory)
        {
            var sharedAssembly = typeof(BootstrapperBase).GetTypeInfo().Assembly;
            RegisterViews(viewFactory, sharedAssembly);

            var assembly = GetType().GetTypeInfo().Assembly;
            RegisterViews(viewFactory, assembly);
        }

        private void RegisterViews(ViewFactory viewFactory, Assembly assembly)
        {
            var views = assembly.DefinedTypes.Where(t => t.IsPage());

            RegisterViewModels(viewFactory, assembly, views);
        }

        private void RegisterViewModels(ViewFactory viewFactory, Assembly assembly, IEnumerable<TypeInfo> views)
        {
            foreach (var view in views)
            {
                log.Info($"Looking for viewmodels for view {view.Name} in {assembly.FullName}");
                var viewModel = assembly.DefinedTypes.SingleOrDefault(type => type.IsViewModelFor(view));

                if (viewModel == null)
                    log.Warn($"No viewmodel found for type {view.Name}");

                log.Info($"Registering viewmodel {viewModel.Name} for view {view.Name}");
                viewFactory.Register(view.AsType(), viewModel.AsType());
            }
        }
    }

    internal static class Extensions
    {
        internal static bool IsPage(this TypeInfo type)
        {
            return typeof(Page).GetTypeInfo().IsAssignableFrom(type);
        }

        internal static bool IsViewModelFor(this TypeInfo type, TypeInfo view)
        {
            return type.Name.StartsWith(view.Name) &&
                (type.Name.EndsWith("ViewModel") ||
                 type.Name.EndsWith("VM"));
        }
    }
}