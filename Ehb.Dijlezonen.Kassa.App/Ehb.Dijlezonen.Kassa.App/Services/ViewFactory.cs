using Ehb.Dijlezonen.Kassa.Infrastructure;
using System;
using System.Collections.Generic;
using Common.Logging;
using Xamarin.Forms;
using Autofac;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class ViewFactory
    {
        /// <summary>
        /// Key value pair of types to indicate the relation between ViewModel and View.
        /// Key = ViewModel
        /// Value = View
        /// </summary>
        private readonly IDictionary<Type, Type> map;
        private readonly ILog log;

        public ViewFactory(Logging logging)
        {
            this.log = logging.GetLoggerFor<ViewFactory>();
            map = new Dictionary<Type, Type>();
        }

        public void Register(Type view, Type viewModel)
        {
            map.Add(viewModel, view);
            log.Info($"Registered '{viewModel.Name}' for '{view.Name}'");
        }

        public Page ResolveViewFor<TViewModel>()
        {
            var viewType = map[typeof(TViewModel)];

            var container = IoC.Container;

            var view = container.Resolve(viewType) as Page;
            var vm = container.Resolve<TViewModel>();

            view.BindingContext = vm;

            log.Info($"Resolved view '{view.GetType().Name}' for viewmodel '{vm.GetType().Name}'");
            return view;
        }
    }
}
