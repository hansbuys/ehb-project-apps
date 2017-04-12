﻿using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.App.UWP.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.UWP
{
    public class Builder : AppBuilderBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new WindowsLoggerFactoryAdapter()));
        }

        protected override void RegisterComponents(ContainerBuilder builder)
        {
        }
    }
}