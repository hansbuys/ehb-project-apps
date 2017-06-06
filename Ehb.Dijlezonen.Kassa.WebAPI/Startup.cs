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
            var configureMvcOptions = Configuration.ReadOptions<ConfigureMvcOptions>();
            services.ConfigureWithMvc(configureMvcOptions);

            var tokenOptions = Configuration.ReadOptions<TokenAuthenticationOptions>("TokenAuthentication", services: services);
            
            var connection = @"Server=(localdb)\mssqllocaldb;Database=Ehb.Dijlezonen.Kassa;Trusted_Connection=True;";
            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            Container = services.SetupAutofac(builder =>
            {
                builder.RegisterType<IdentityResolver>().As<IIdentityResolver>();

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
            IApplicationLifetime appLifetime, IIdentityResolver resolver, IOptions<TokenAuthenticationOptions> tokenOptions, 
            IDbContextInitializer dbInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.SetupJwtBearerAuth(tokenOptions, resolver);

            app.UseMvc();

            dbInitializer.Initialize();

            appLifetime.ApplicationStopped.Register(() => Container.Dispose());
        }
    }

    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Reads settings from appsettings into a strongly typed object.
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="configuration">the configuration root to read the settings from</param>
        /// <param name="sectionName">the name of the setting used in the appsettings file. Defaults to the name of <typeparam name="TOptions"></typeparam></param>
        /// <param name="services">when provided it will register the options to the services container.</param>
        /// <returns></returns>
        public static TOptions ReadOptions<TOptions>(this IConfigurationRoot configuration, string sectionName = null, IServiceCollection services = null)
            where TOptions : class, new()
        {
            var options = new TOptions();

            var tokenAuth = configuration.GetSection(sectionName ?? typeof(TOptions).Name);
            tokenAuth.Bind(options);

            services?.Configure<TokenAuthenticationOptions>(tokenAuth);

            return options;
        }
    }
}
