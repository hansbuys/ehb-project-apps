using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Common.Logging;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class NavigationService
    {
        private readonly INavigation navigation;
        private readonly ILog log;

        /// <summary>
        /// Key value pair of types to indicate the relation between ViewModel and View.
        /// Key = ViewModel
        /// Value = View
        /// </summary>
        private readonly IDictionary<Type, Type> map;

        public NavigationService(Logging logging, INavigation navigation)
        {
            this.log = logging.GetLoggerFor<NavigationService>();
            this.navigation = navigation;

            map = new Dictionary<Type, Type>();
        }

        public void Register(Type view, Type viewModel)
        {
            map.Add(viewModel, view);
            log.Info($"Registered '{viewModel.Name}' for '{view.Name}'");
        }

        public Task NavigateTo<TViewModel>()
        {
            Page view = ResolveViewFor<TViewModel>();

            log.Info($"Navigating to {view.GetType().Name}");

            return navigation.PushAsync(view);
        }

        public Task GoToMain()
        {
            return navigation.PopToRootAsync();
        }

        public Task NavigateToModal<TViewModel>()
        {
            Page view = ResolveViewFor<TViewModel>();

            log.Info($"Navigating modally to {view.GetType().Name}");

            return navigation.PushModalAsync(view);
        }

        private Page ResolveViewFor<TViewModel>()
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