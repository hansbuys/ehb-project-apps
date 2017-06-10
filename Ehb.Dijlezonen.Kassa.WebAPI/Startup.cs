using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication;
using Ehb.Dijlezonen.Kassa.WebAPI.Authentication.Storage;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration;
using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer Container { get; private set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.ConfigureWithMvc(Configuration);
            
            SetupDatabaseConnection(services);

            return SetContainerAndCreateServiceProvider(services);
        }

        protected virtual void SetupDatabaseConnection(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=Ehb.Dijlezonen.Kassa;Trusted_Connection=True;";
            services.AddDbContext<UserContext>(options => { options.UseSqlServer(connection); });
        }

        private IServiceProvider SetContainerAndCreateServiceProvider(IServiceCollection services)
        {
            var tokenOptions = Configuration.ReadOptions<TokenAuthenticationOptions>("TokenAuthentication", services);

            services.UseJwtTokenGenerator(tokenOptions);

            Container = services.SetupAutofac(builder =>
            {
                builder.RegisterType<IdentityRepository>().As<IIdentityRepository>();

                if (tokenOptions.UseFakeCredentials)
                {
                    builder.RegisterType<TestUserContextInitializer>().As<IDbContextInitializer>();
                }
                else
                {
                    builder.RegisterType<UserContextInitializer>().As<IDbContextInitializer>();
                }
            });

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime, IOptions<TokenProviderOptions> tokenProviderOptions, 
            IOptions<TokenAuthenticationOptions> tokenAuthenticationOptions, IDbContextInitializer dbInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.SetupJwtTokenGenerator(tokenAuthenticationOptions.Value);
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions.Value));

            app.UseMvc();

            dbInitializer.Initialize();

            appLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }
    }
}
