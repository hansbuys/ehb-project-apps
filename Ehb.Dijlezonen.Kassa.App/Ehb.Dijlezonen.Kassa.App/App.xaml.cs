using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public partial class App
    {
        public App(BootstrapperBase bootstrapper, AccountStore accountStore)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            var accountStoreAdapter = new AccountStoreAdapter(accountStore);
            var navigationAdapter = new NavigationAdapter(MainPage.Navigation);
            var container = bootstrapper.Initialize(navigationAdapter, accountStoreAdapter);

            var logging = container.Resolve<Logging>();
            var logger = logging.GetLoggerFor<App>();

            logger.Debug("Loaded IoC container, starting app...");

            var viewModel = container.Resolve<MainPageViewModel>();
            MainPage.BindingContext = viewModel;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}