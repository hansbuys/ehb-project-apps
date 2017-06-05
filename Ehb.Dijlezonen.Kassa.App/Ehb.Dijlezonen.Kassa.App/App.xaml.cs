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
        private readonly Bootstrapper bootstrapper;
        private readonly ILog log;
        
        public App(Bootstrapper bootstrapper)
        {
            InitializeComponent();

            MainPage = new NavigationPage();
            this.bootstrapper = bootstrapper;

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

                c.RegisterType<BackendConfiguration>().As<IBackendConfiguration>();
                c.RegisterType<LoginProvider>().As<ILoginProvider>().SingleInstance();
            });
        }

        protected override void OnStart()
        {
            log.Debug("Starting application.");

            var nav = container.Resolve<Navigation>();
            nav.NavigateTo<MainPageViewModel>().GetAwaiter().GetResult();
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