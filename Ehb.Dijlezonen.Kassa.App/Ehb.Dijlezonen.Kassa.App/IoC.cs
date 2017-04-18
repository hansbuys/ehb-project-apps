using System;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Ehb.Dijlezonen.Kassa.App.Shared
{
    public static class IoC
    {
        private static IContainer container;
        public static IContainer Container
        {
            get
            {
                if (container == null)
                    throw new InvalidOperationException($"You need to call {nameof(InitializeContainer)} first.");

                return container;
            }
        }

        public static IContainer InitializeContainer(Action<ContainerBuilder> configure)
        {
            var c = new ContainerBuilder();

            c.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            configure(c);

            container = c.Build();

            return container;
        }
    }
}