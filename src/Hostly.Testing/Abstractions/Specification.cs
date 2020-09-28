using System;
using System.Threading.Tasks;
using Hostly.Navigation;
using Hostly.Testing.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Hostly.Testing.Abstractions
{
    public abstract class Specification<TResult> : IAsyncLifetime
    {
        protected readonly IXamarinHostBuilder _xamarinHostBuilder;
        protected IXamarinHost Host { get; private set; }
        protected abstract Task<TResult> Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }
        protected TResult Result { get; private set; }

        protected IServiceProvider ServiceProvider => Host.Services;

        protected virtual void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services) { }
        protected virtual void ConfigureNavigationServices(XamarinHostBuilderContext ctx, IServiceProvider serviceProvider, INavigationBuilder navigationBuilder) { }
        protected virtual void ConfigureHost(IXamarinHostBuilder hostBuilder) 
        {
            hostBuilder.UseTestPlatformServices();
        }

        public Specification()
        {
            _xamarinHostBuilder = new XamarinHostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureNavigation(ConfigureNavigationServices);

            ConfigureHost(_xamarinHostBuilder);
        }

        public virtual async Task DisposeAsync()
        {
            var hostLifetime = Host?.Services.GetRequiredService<IHostApplicationLifetime>();

            try
            {
                if (hostLifetime != null
                    && hostLifetime.ApplicationStarted.IsCancellationRequested
                    && !hostLifetime.ApplicationStopping.IsCancellationRequested
                    && !hostLifetime.ApplicationStopped.IsCancellationRequested)
                    await Host.StopAsync();
            }
            catch (OperationCanceledException)
            {
                // Should be hit as the cancelation token in hosted services will throw;
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task InitializeAsync()
        {
            await When();

            try
            {
                Host = _xamarinHostBuilder.Build();
                Result = await Given();
            }
            catch (Exception e)
            {
                if (ExceptionMode == ExceptionMode.Record)
                    Exception = e;
                else
                    throw;
            }
        }
    }
    public abstract class Specification : IAsyncLifetime
    {
        protected readonly IXamarinHostBuilder _xamarinHostBuilder;
        protected IXamarinHost Host { get; private set; }
        protected abstract Task Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }

        protected IServiceProvider ServiceProvider => Host.Services;

        protected virtual void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services) { }
        protected virtual void ConfigureNavigationServices(XamarinHostBuilderContext ctx, IServiceProvider serviceProvider, INavigationBuilder navigationBuilder) { }
        protected virtual void ConfigureHost(IXamarinHostBuilder hostBuilder)
        {
            hostBuilder.UseTestPlatformServices();
        }

        public Specification()
        {
            _xamarinHostBuilder = new XamarinHostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureNavigation(ConfigureNavigationServices);

            ConfigureHost(_xamarinHostBuilder);
        }

        public virtual async Task DisposeAsync()
        {
            var hostLifetime = Host?.Services.GetRequiredService<IHostApplicationLifetime>();

            try
            {
                if (hostLifetime != null
                    && hostLifetime.ApplicationStarted.IsCancellationRequested
                    && !hostLifetime.ApplicationStopping.IsCancellationRequested
                    && !hostLifetime.ApplicationStopped.IsCancellationRequested)
                    await Host.StopAsync();
            }
            catch (OperationCanceledException)
            {
                // Should be hit as the cancelation token in hosted services will throw;
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task InitializeAsync()
        {
            await When();

            try
            {
                Host = _xamarinHostBuilder.Build();
                await Given();
            }
            catch (Exception e)
            {
                if (ExceptionMode == ExceptionMode.Record)
                    Exception = e;
                else
                    throw;
            }
        }
    }
}
