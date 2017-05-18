using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public partial class App
    {
        public App(BootstrapperBase bootstrapper, AccountStore accountStore)
        {
            InitializeComponent();

            MainPage = new NavigationPage();

            var container = bootstrapper.Initialize(c =>
            {
                c.RegisterInstance(accountStore);
                c.RegisterInstance(MainPage.Navigation).As<INavigation>();
                c.RegisterType<NavigationAdapter>().As<INavigationAdapter>();
                c.RegisterType<AccountStoreAdapter>().As<IAccountStore>();
            });

            var nav = container.Resolve<INavigationAdapter>();
            nav.NavigateTo<MainPageViewModel>();
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