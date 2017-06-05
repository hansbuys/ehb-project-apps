using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    internal static class MvcExtensions
    {
        public static void ConfigureWithMvc(this IServiceCollection services, IConfigurationSection mvcOptions)
        {

            services.AddMvc(options =>
            {
                if (mvcOptions.GetSection("RequireHttps").Value == "True")
                    options.Filters.Add(new RequireHttpsAttribute());
            });
        }
    }
}