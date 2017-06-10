using System.Threading.Tasks;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface INavigationAdapter
    {
        Task<TViewModel> NavigateTo<TViewModel>();

        Task<TViewModel> NavigateToModal<TViewModel>();
        Task CloseModal();
        Task Close();
    }
}