using Ehb.Dijlezonen.Kassa.WebAPI.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Configuration
{
    public static class ConfigurationRootExtensions
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