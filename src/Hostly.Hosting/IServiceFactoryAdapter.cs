using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly
{
    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}
