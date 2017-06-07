using Autofac;
using Common.Logging;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public partial class App
    {
        private IContainer container;
        private readonly BootstrapperBase bootstrapper;
        private readonly ILog log;

        public App(BootstrapperBase bootstrapper)
        {
            this.bootstrapper = bootstrapper;

            InitializeComponent();

            MainPage = new NavigationPage();

            InitializeContainer();

            var logging = container.Resolve<Logging>();
            log = logging.GetLoggerFor<App>();

            log.Debug("Container has been initialized.");
        }

        private void InitializeContainer()
        {
            container = bootstrapper.Initialize(c =>
            {
                c.RegisterInstance(MainPage.Navigation).As<INavigation>();
                c.RegisterType<NavigationAdapter>().As<INavigationAdapter>();

                c.RegisterType<LoginProvider>().As<ILoginProvider>().SingleInstance();
            });
        }

        protected override void OnStart()
        {
            log.Debug("Starting application.");

            var nav = container.Resolve<Navigation>();

            Device.BeginInvokeOnMainThread(async () => await nav.NavigateTo<MainPageViewModel>());
        }

        protected override void OnSleep()
        {
            log.Debug("Application going to sleep.");
        }

        protected override void OnResume()
        {
            log.Debug("Application resuming.");
        }
    }
}