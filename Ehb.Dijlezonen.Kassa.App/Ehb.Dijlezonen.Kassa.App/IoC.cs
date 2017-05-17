using System;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public static class IoC
    {
        public static IContainer InitializeContainer(Action<ContainerBuilder> configure)
        {
            var c = new ContainerBuilder();

            c.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            configure(c);

            return c.Build();
        }
    }
}