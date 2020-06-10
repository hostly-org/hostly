using System;
using System.Collections.Generic;
using System.IO;
using Hostly.Extensions;
using Hostly.Internals;
using Hostly.Navigation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Xamarin.Forms;

namespace Hostly
{
    /// <summary>
    /// A builder for <see cref="IXamarinHost"/>
    /// </summary>
    public class XamarinHostBuilder : IXamarinHostBuilder
    {
        private List<Action<IConfigurationBuilder>> _configureHostConfigActions = new List<Action<IConfigurationBuilder>>();
        private List<Action<XamarinHostBuilderContext, IServiceProvider, INavigationBuilder>> _configureNavigationConfigActions = new List<Action<XamarinHostBuilderContext, IServiceProvider, INavigationBuilder>>();
        private List<Action<XamarinHostBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new List<Action<XamarinHostBuilderContext, IConfigurationBuilder>>();
        private List<Action<XamarinHostBuilderContext, IServiceCollection>> _configureServicesActions = new List<Action<XamarinHostBuilderContext, IServiceCollection>>();
        private List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();
        private IServiceFactoryAdapter _serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());
        private bool _hostBuilt;
        private IConfiguration _hostConfiguration;
        private IConfiguration _appConfiguration;
        private XamarinHostBuilderContext _hostBuilderContext;
        private IXamarinHostEnvironment _hostEnvironment;
        private IServiceProvider _appServices;

        /// <summary>
        /// Adds a delegate for configuring additional services for the <see cref="IXamarinHost"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureHostConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring configuration for the <see cref="IXamarinHost"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder ConfigureNavigation(Action<XamarinHostBuilderContext, IServiceProvider, INavigationBuilder> configureDelegate)
        {
            _configureNavigationConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring configuration for the <see cref="IXamarinHost"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder ConfigureAppConfiguration(Action<XamarinHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring additional services for the Xamarin host. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder ConfigureServices(Action<XamarinHostBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Registers the <see cref="IServiceProviderFactory{TContainerBuilder}"/>
        /// </summary>
        /// <param name="factory">The <see cref="IServiceProviderFactory{TContainerBuilder}"/> to be registered .</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        /// <summary>
        /// Adds a delegate for configuring the <typeparamref name="TContainerBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="configureDelegate">A delegate for configuring the <typeparamref name="TContainerBuilder"/>.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public IXamarinHostBuilder ConfigureContainer<TContainerBuilder>(Action<XamarinHostBuilderContext, TContainerBuilder> configureDelegate)
        {
            _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                ?? throw new ArgumentNullException(nameof(configureDelegate))));
            return this;
        }

        /// <summary>
        /// Builds the <see cref="IXamarinHost"/>
        /// </summary>
        /// <returns>The <see cref="IXamarinHost"/>.</returns>
        public IXamarinHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }
            _hostBuilt = true;

            BuildHostConfiguration();
            CreateHostingEnvironment();
            CreateHostBuilderContext();
            BuildAppConfiguration();
            CreateServiceProvider();

            return _appServices.GetRequiredService<IXamarinHost>();
        }

        private void BuildHostConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            foreach (var buildAction in _configureHostConfigActions)
            {
                buildAction(configBuilder);
            }
            _hostConfiguration = configBuilder.Build();
        }

        private void CreateHostingEnvironment()
        {
            _hostEnvironment = new XamarinHostEnvironment
            {
                ApplicationName = _hostConfiguration[XamarinHostDefaults.ApplicationKey],
                EnvironmentName = _hostConfiguration[XamarinHostDefaults.EnvironmentKey] ?? Environments.Production,
                ContentRootPath = ResolveContentRootPath(_hostConfiguration[HostDefaults.ContentRootKey], AppContext.BaseDirectory),
                DevicePlatform = Device.RuntimePlatform
            };
            _hostEnvironment.ContentRootFileProvider = new PhysicalFileProvider(_hostEnvironment.ContentRootPath);
        }

        private string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return basePath;
            }
            if (Path.IsPathRooted(contentRootPath))
            {
                return contentRootPath;
            }
            return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }

        private void CreateHostBuilderContext()
        {
            _hostBuilderContext = new XamarinHostBuilderContext
            {
                HostEnvironment = _hostEnvironment,
                Configuration = _hostConfiguration
            };
        }

        private void BuildAppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddConfiguration(_hostConfiguration);
            foreach (var buildAction in _configureAppConfigActions)
            {
                buildAction(_hostBuilderContext, configBuilder);
            }
            _appConfiguration = configBuilder.Build();
            _hostBuilderContext.Configuration = _appConfiguration;
        }

        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_hostEnvironment);
            services.AddSingleton(_hostBuilderContext);
            services.AddSingleton(_appConfiguration);
            services.AddSingleton<IHostApplicationLifetime, ApplicationLifetime>();
            services.AddSingleton<IHostLifetime, XamarinHostLifetime>();
            services.AddSingleton<IXamarinHost, XamarinHost>();
            services.AddSingleton<XamarinNavigationProxy>();
            services.AddSingleton<INavigation>(sp => sp.GetRequiredService<XamarinNavigationProxy>());
            services.AddSingleton<INavigationProxyBuilder, NavigationProxyBuilder>();
            services.AddSingleton<IExtendedNavigationBuilder, NavigationBuilder>();

            services.AddOptions();
            services.AddLogging();

            foreach (var configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_hostBuilderContext, services);
            }

            var startup = services.BuildServiceProvider().GetService<IXamarinStartup>();

            if (startup != null)
                startup.ConfigureServices(_hostBuilderContext, services);

            var containerBuilder = _serviceProviderFactory.CreateBuilder(services);

            foreach (var containerAction in _configureContainerActions)
            {
                containerAction.ConfigureContainer(_hostBuilderContext, containerBuilder);
            }

            // If no navigation root has been configured, then set it to the application
            services.TryAddSingleton<XamarinNavigationRootDelegate>(sp => () => sp.GetRequiredService<XamarinApplicationDelegate>()());

            _appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            if (_appServices == null)
            {
                throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
            }

            if(_appServices.GetRequiredService<IXamarinHostingPlatform>() == null)
            {
                throw new InvalidOperationException($"Please register an instance of {nameof(IXamarinHostingPlatform)}, this can be done using the {nameof(IXamarinHostBuilder)}.{nameof(XamarinHostBuilderExtensions.UsePlatform)} extension method");
            }

            if (_appServices.GetRequiredService<XamarinApplicationDelegate>() == null)
            {
                throw new InvalidOperationException($"Please register an instance of your application, this can be done using the {nameof(IXamarinHostBuilder)}.{nameof(XamarinHostBuilderExtensions.UseApplication)} extension method");
            }

            var navigationBuilder = _appServices.GetRequiredService<IExtendedNavigationBuilder>();

            foreach (var navigationAction in _configureNavigationConfigActions)
            {
                navigationAction(_hostBuilderContext, _appServices, navigationBuilder);
            }

            navigationBuilder.Build();
        }
    }
}
