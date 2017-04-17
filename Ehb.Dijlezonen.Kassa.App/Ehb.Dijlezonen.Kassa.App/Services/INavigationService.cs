using System;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface INavigationService
    {
        void Register(Type view, Type viewModel);

        Task NavigateTo<T>()
            where T : class;
    }
}