using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public class NavigationService : INavigationService
    {
        private readonly INavigation navigation;

        /// <summary>
        /// Key value pair of types to indicate the relation between ViewModel and View.
        /// Key = ViewModel
        /// Value = View
        /// </summary>
        private readonly IDictionary<Type, Type> map;

        public NavigationService(INavigation navigation)
        {
            this.navigation = navigation;

            map = new Dictionary<Type, Type>();
        }

        public void Register(Type view, Type viewModel)
        {
            map.Add(viewModel, view);
        }

        Task INavigationService.NavigateTo<T>()
        {
            var viewType = map[typeof(T)];

            var container = IoC.Container;

            var view = container.Resolve(viewType) as Page;
            var vm = container.Resolve<T>();

            if (view == null)
                throw new Exception($"Unable to resolve {typeof(T).Name} to a View of type Page");

            view.BindingContext = vm;

            return navigation.PushAsync(view);
        }
    }
}