using System;
using Autofac;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public partial class App
    {
        public App(AppBuilderBase appBuilder)
        {
            InitializeComponent();

            MainPage = new MainPage();

            using (var container = appBuilder.StartContainer())
            {
                var logging = container.Resolve<Logging>();
                var logger = logging.GetLoggerFor<App>();

                logger.Debug("Loaded IoC container, starting app...");



                logger.Debug("Done.");
            }
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