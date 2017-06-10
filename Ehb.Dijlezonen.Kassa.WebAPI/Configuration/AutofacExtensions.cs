using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Ehb.Dijlezonen.Kassa.WebAPI.Configuration
{
    public static class AutofacExtensions
    {
        public static IContainer SetupAutofac(this IServiceCollection services, Action<ContainerBuilder> configureDependencies = null)
        {
            var bootstrapper = new ApiBootstrapper();
            return bootstrapper.Initialize(x =>
            {
                x.Populate(services);
                configureDependencies?.Invoke(x);
            });
        }
    }
}