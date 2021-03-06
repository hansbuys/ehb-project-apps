﻿using Autofac;
using Ehb.Dijlezonen.Kassa.App.Shared;
using Ehb.Dijlezonen.Kassa.App.Shared.Services;
using Ehb.Dijlezonen.Kassa.App.UWP.Services;
using Ehb.Dijlezonen.Kassa.Infrastructure;

namespace Ehb.Dijlezonen.Kassa.App.UWP
{
    public class Bootstrapper : AppBootstrapperBase
    {
        protected override void RegisterPorts(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Logging(new WindowsLoggerFactoryAdapter()));
            builder.RegisterType<WindowsBackendConfiguration>().As<IBackendConfiguration>();
        }
    }
}