using System.Threading.Tasks;
using System.Windows.Input;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel
    {
        private readonly NavigationService navigation;
        private readonly IAccountStore auth;

        public MainPageViewModel(NavigationService navigation, IAccountStore auth)
        {
            this.navigation = navigation;
            this.auth = auth;

            Initialize().Wait();
        }

        private async Task Initialize()
        {
            if (!await IsLoggedIn().ConfigureAwait(false))
            {
                await navigation.NavigateToModal<LoginViewModel>();
            }
        }

        private Task<bool> IsLoggedIn()
        {
            return auth.IsLoggedIn();
        }

        public ICommand NavigateToSecondStageCommand => new Command(async () => await NavigateToSecondStage().ConfigureAwait(false));
        public string Title => "De Dijlezonen Kassa";
        public string NavigateToSecondStageCommandText => "Ga naar volgende";

        private Task NavigateToSecondStage()
        {
            return navigation.NavigateTo<SecondStageViewModel>();
        }
    }
}
