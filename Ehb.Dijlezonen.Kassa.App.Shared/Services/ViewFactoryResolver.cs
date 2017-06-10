using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class ViewFactoryResolver
    {
        private readonly ILog log;

        public ViewFactoryResolver(Logging logging)
        {
            log = logging.GetLoggerFor<ViewFactoryResolver>();
        }

        public void RegisterViews(ViewFactory viewFactory)
        {
            var sharedAssembly = GetType().GetTypeInfo().Assembly;
            RegisterViews(viewFactory, sharedAssembly);
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