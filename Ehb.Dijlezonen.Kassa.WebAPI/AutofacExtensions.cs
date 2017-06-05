using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Ehb.Dijlezonen.Kassa.WebAPI
{
    public static class AutofacExtensions
    {
        public static IContainer AddAutofac(this IServiceCollection services)
        {
            var bootstrapper = new ApiBootstrapper();
            return bootstrapper.Initialize(x =>
            {
                x.Populate(services);
            });
        }
    }
}