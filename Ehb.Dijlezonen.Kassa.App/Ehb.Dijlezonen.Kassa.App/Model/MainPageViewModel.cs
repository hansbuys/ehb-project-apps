using System.Threading.Tasks;
using System.Windows.Input;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel
    {
        private readonly INavigationService navigation;

        public MainPageViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
        }

        public ICommand NavigateToSecondStageCommand => new Command(async () => await NavigateToSecondStage().ConfigureAwait(false), () => true);

        private Task NavigateToSecondStage()
        {
            return navigation.NavigateTo<SecondStageViewModel>();
        }
    }
}
