﻿using Autofac;
using Ehb.Dijlezonen.Kassa.App.iOS.Services;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.iOS
{
    public class Builder : AppBuilderBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new iOSLoggerFactoryAdapter()));
        }

        protected override void RegisterComponents(ContainerBuilder builder)
        {
        }
    }
}