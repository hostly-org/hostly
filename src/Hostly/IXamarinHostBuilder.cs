using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hostly
{
    /// <summary>
    /// A builder for <see cref="IXamarinHost"/>
    /// </summary>
    public interface IXamarinHostBuilder
    {
        /// <summary>
        /// Adds a delegate for configuring additional services for the <see cref="IXamarinHost"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        IXamarinHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);
        /// <summary>
        /// Adds a delegate for configuring configuration for the <see cref="IXamarinHost"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        IXamarinHostBuilder ConfigureAppConfiguration(Action<XamarinHostBuilderContext, IConfigurationBuilder> configureDelegate);
        /// <summary>
        /// Adds a delegate for configuring additional services for the Xamarin host. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        IXamarinHostBuilder ConfigureServices(Action<XamarinHostBuilderContext, IServiceCollection> configureDelegate);
        /// <summary>
        /// Registers the <see cref="IServiceProviderFactory{TContainerBuilder}"/>
        /// </summary>
        /// <param name="factory">The <see cref="IServiceProviderFactory{TContainerBuilder}"/> to be registered .</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        IXamarinHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory);
        /// <summary>
        /// Adds a delegate for configuring the <typeparamref name="TContainerBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <typeparamref name="TContainerBuilder"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        IXamarinHostBuilder ConfigureContainer<TContainerBuilder>(Action<XamarinHostBuilderContext, TContainerBuilder> configureDelegate);
        /// <summary>
        /// Builds the <see cref="IXamarinHost"/>
        /// </summary>
        /// <returns>The <see cref="IXamarinHost"/>.</returns>
        IXamarinHost Build();
    }
}
