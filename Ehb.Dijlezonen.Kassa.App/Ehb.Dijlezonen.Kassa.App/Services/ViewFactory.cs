using System;
using System.Collections.Generic;
using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class ViewFactory
    {
        /// <summary>
        ///     Key value pair of types to indicate the relation between ViewModel and View.
        ///     Key = ViewModel
        ///     Value = View
        /// </summary>
        private readonly IDictionary<Type, Type> map;
        private readonly ILog log;

        public ViewFactory(Logging logging)
        {
            log = logging.GetLoggerFor<ViewFactory>();
            map = new Dictionary<Type, Type>();
        }

        protected IContainer Container { get; private set; }

        public void SetResolver(IContainer container)
        {
            if (Container != null)
                throw new Exception("Resolver has already been set!");

            Container = container;
        }

        public void Register(Type view, Type viewModel)
        {
            map.Add(viewModel, view);
            log.Info($"Registered '{viewModel.Name}' for '{view.Name}'");
        }

        public Page ResolveViewFor<TViewModel>()
        {
            var viewType = map[typeof(TViewModel)];

            var view = Container.Resolve(viewType) as Page;
            var vm = Container.Resolve<TViewModel>();

            view.BindingContext = vm;

            log.Info($"Resolved view '{view.GetType().Name}' for viewmodel '{vm.GetType().Name}'");
            return view;
        }
    }
}