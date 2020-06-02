using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Internals
{
    internal sealed class StartupMethods
    {
        public StartupMethods(object instance, Action<XamarinHostBuilderContext, IServiceCollection> configureServices)
        {
            StartupInstance = instance;
            ConfigureServicesDelegate = configureServices;
        }

        public object StartupInstance { get; }
        public Action<XamarinHostBuilderContext, IServiceCollection> ConfigureServicesDelegate { get; }
    }
}
