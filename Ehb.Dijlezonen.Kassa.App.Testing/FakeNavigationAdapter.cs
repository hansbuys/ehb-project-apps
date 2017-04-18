using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Autofac;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public class FakeNavigationAdapter : INavigationAdapter
    {
        public List<object> ModalStack { get; } = new List<object>();
        public List<object> NavigationStack { get; } = new List<object>();
        
        Task INavigationAdapter.NavigateTo<TViewModel>()
        {
            NavigationStack.Add(IoC.Container.Resolve<TViewModel>());
            return Task.FromResult(0);
        }

        Task INavigationAdapter.NavigateToModal<TViewModel>()
        {
            ModalStack.Add(IoC.Container.Resolve<TViewModel>());
            return Task.FromResult(0);
        }

        Task INavigationAdapter.CloseModal()
        {
            var viewModel = ModalStack.LastOrDefault();
            if (viewModel != null)
                ModalStack.Remove(viewModel);
            return Task.FromResult(0);
        }
    }
}