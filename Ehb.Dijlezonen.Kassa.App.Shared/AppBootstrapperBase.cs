using System.Linq;
using System.Reflection;
using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Common.Logging;
using System;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public abstract class AppBootstrapperBase : BootstrapperBase
    {
        private ILog log;
        
        protected override void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().SingleInstance();
            builder.RegisterType<UserService>().SingleInstance();
        }

        public override IContainer Initialize(Action<ContainerBuilder> addDependencies = null)
        {
            var container = base.Initialize(addDependencies);
            
            var logging = container.Resolve<Logging>();
            log = logging.GetLoggerFor<AppBootstrapperBase>();

            var viewFactory = container.Resolve<ViewFactory>();
            RegisterViews(viewFactory);

            return container;
        }

        private void RegisterViews(ViewFactory viewFactory)
        {
            var sharedAssembly = typeof(AppBootstrapperBase).GetTypeInfo().Assembly;
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
                {
                    log.Warn($"No viewmodel found for type {view.Name}");
                }
                else
                {
                    log.Info($"Registering viewmodel {viewModel.Name} for view {view.Name}");
                    viewFactory.Register(view.AsType(), viewModel.AsType());
                }
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