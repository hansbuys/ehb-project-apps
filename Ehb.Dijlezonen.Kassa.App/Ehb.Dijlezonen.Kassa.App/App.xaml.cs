using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public partial class App
    {
        public App(BootstrapperBase bootstrapper)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            
            var container = bootstrapper.StartContainer(MainPage.Navigation);
            
            var logging = container.Resolve<Logging>();
            var logger = logging.GetLoggerFor<App>();

            logger.Debug("Loaded IoC container, starting app...");

            var navigation = container.Resolve<INavigationService>();
            bootstrapper.RegisterViews(navigation);

            var viewModel = container.Resolve<MainPageViewModel>();
            MainPage.BindingContext = viewModel;

            logger.Debug("Done.");
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