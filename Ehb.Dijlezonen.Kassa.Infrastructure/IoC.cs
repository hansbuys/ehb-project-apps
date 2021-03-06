﻿using System;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Ehb.Dijlezonen.Kassa.Infrastructure
{
    internal static class IoC
    {
        internal static IContainer InitializeContainer(Action<ContainerBuilder> configure)
        {
            var c = new ContainerBuilder();

            c.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            configure(c);

            return c.Build();
        }
    }
}