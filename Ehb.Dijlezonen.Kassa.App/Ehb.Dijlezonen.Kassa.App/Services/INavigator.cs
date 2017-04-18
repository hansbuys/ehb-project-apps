using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface INavigationAdapter
    {
        Task NavigateTo<TViewModel>();

        Task NavigateToModal<TViewModel>();
        Task CloseModal();
    }
}