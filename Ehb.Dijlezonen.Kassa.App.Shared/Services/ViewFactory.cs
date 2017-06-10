using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class ViewFactory
    {
        private readonly IDictionary<Type, Type> mapViewModelToView;
        private readonly IDictionary<Type, Type> mapViewToViewModel;
        private readonly IComponentContext container;
        private readonly ILog log;
        
        public ViewFactory(Logging logging, IComponentContext container, ViewFactoryResolver resolver)
        {
            this.container = container;
            log = logging.GetLoggerFor<ViewFactory>();
            mapViewModelToView = new Dictionary<Type, Type>();
            mapViewToViewModel = new Dictionary<Type, Type>();

            resolver.RegisterViews(this);
        }

        public void Register(Type view, Type viewModel)
        {
            mapViewModelToView.Add(viewModel, view);
            mapViewToViewModel.Add(view, viewModel);
        }

        public Page ResolveViewFor<TViewModel>()
        {
            var viewType = mapViewModelToView[typeof(TViewModel)];

            var view = container.Resolve(viewType) as Page;

            ResolveChildViews(view);

            var vm = container.Resolve<TViewModel>();

            view.BindingContext = vm;

            log.Info($"Resolved view '{view.GetType().Name}' for viewmodel '{vm.GetType().Name}'");
            return view;
        }

        private void ResolveChildViews(Page view)
        {
            var page = view as TabbedPage;
            if (page != null)
            {
                var tabbedView = page;
                tabbedView.Children.ToList().ForEach(childView =>
                {
                    var childViewModelType = mapViewToViewModel[childView.GetType()];

                    var childViewModel = container.Resolve(childViewModelType);
                    childView.BindingContext = childViewModel;
                });
            }
        }
    }
}