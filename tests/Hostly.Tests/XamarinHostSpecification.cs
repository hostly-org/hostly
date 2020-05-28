using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace Hostly.Tests
{
    public abstract class XamarinHostSpecification<TResult> : IAsyncLifetime
    {
        protected readonly IXamarinHostBuilder _xamarinHostBuilder;
        protected IXamarinHost Host { get; private set; }
        protected abstract Task<TResult> Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }
        protected TResult Result { get; private set; }
        private bool HostRunning { get; set; }

        protected virtual void BuildServices(IServiceCollection services) { }

        public XamarinHostSpecification()
        {
            _xamarinHostBuilder = new XamarinHostBuilder();

            Device.PlatformServices = new MockPlatformServices();
        }

        public async Task DisposeAsync()
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

        public async Task InitializeAsync()
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
                    throw e;
            }
        }
    }

    public abstract class XamarinHostSpecification : IAsyncLifetime
    {
        protected readonly IXamarinHostBuilder _xamarinHostBuilder;
        protected IXamarinHost Host { get; private set; }
        protected abstract Task Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }

        protected virtual void BuildServices(IServiceCollection services) { }

        public XamarinHostSpecification()
        {
            _xamarinHostBuilder = new XamarinHostBuilder();

            Device.PlatformServices = new MockPlatformServices();
        }

        public async Task DisposeAsync()
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
            catch(OperationCanceledException)
            {
                // Should be hit as the cancelation token in hosted services will throw;
                return;   
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task InitializeAsync()
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
                    throw e;
            }
        }
    }
}
