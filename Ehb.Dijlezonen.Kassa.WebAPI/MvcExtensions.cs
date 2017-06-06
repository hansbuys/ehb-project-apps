using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    internal static class MvcExtensions
    {
        public static void ConfigureWithMvc(this IServiceCollection services, ConfigureMvcOptions mvcOptions)
        {
            services.AddMvc(options =>
            {
                if (!mvcOptions.AllowInsecureHttp)
                    options.Filters.Add(new RequireHttpsAttribute());
            });
        }
    }
}