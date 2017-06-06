using System.Threading.Tasks;
using System.Windows.Input;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class MainPageViewModel : PropertyChangedViewModelBase, IProtectedViewModel
    {
        private readonly ILoginProvider auth;
        private readonly ILog log;

        public MainPageViewModel(ILoginProvider auth, Logging logging)
        {
            this.auth = auth;
            log = logging.GetLoggerFor<MainPageViewModel>();
        }

        public string Title => "De Dijlezonen Kassa";

        public string LogoutCommandText => "Uitloggen";
        public ICommand LogoutCommand => new Command(async () => await Logout().ConfigureAwait(false));

        public bool IsAdmin { get; set; }

        private async Task Logout()
        {
            log.Debug("Logging out");

            await auth.Logout().ConfigureAwait(false);
        }
    }
}