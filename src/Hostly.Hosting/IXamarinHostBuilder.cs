using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hostly
{
    public interface IXamarinHostBuilder
    {
        IXamarinHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);
        IXamarinHostBuilder ConfigureAppConfiguration(Action<XamarinHostBuilderContext, IConfigurationBuilder> configureDelegate);
        IXamarinHostBuilder ConfigureServices(Action<XamarinHostBuilderContext, IServiceCollection> configureDelegate);
        IXamarinHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory);
        IXamarinHostBuilder ConfigureContainer<TContainerBuilder>(Action<XamarinHostBuilderContext, TContainerBuilder> configureDelegate);
        IXamarinHost Build();
    }
}
