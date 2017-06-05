using System;
using Autofac;
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
        
        public App(Bootstrapper bootstrapper)
        {
            InitializeComponent();

            MainPage = new NavigationPage();
            this.bootstrapper = bootstrapper;

            InitializeContainer();
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
            var nav = container.Resolve<Navigation>();
            nav.NavigateTo<MainPageViewModel>().GetAwaiter().GetResult();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}