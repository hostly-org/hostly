using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hostly.Tests.XamarinHost.StopAsync
{
    public class WhenHostedServicesAreRegistered : XamarinHostSpecification
    {
        private class MockHostedService : IHostedService
        {
            public bool? Running { get; private set; } = null;
            public Task StartAsync(CancellationToken cancellationToken)
            {
                Running = true;
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                Running = false;
                return Task.CompletedTask;
            }
        }

        private class SecondMockHostedService : MockHostedService { }

        protected override async Task Given()
        {
            await Host.StartAsync();
            await Host.StopAsync();
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UseApplication<MockXamarinApplication>()
                .UsePlatform<MockPlatform>()
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHostedService<MockHostedService>();
                    services.AddHostedService<SecondMockHostedService>();
                });

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveHostedServicesRegistered()
        {
            Host.Services.GetService<IEnumerable<IHostedService>>().Should().HaveCount(2);
        }

        [Then]
        public void ShouldNotHaveHostedServicesRunning()
        {
            var hostedServices = Host.Services.GetService<IEnumerable<IHostedService>>();

            foreach (MockHostedService hostedService in hostedServices)
                hostedService.Running.Should().BeFalse();
        }

        [Then]
        public void ShouldHaveApplicationLoadedInPlatform()
        {
            ((MockPlatform)Host.Services.GetRequiredService<IXamarinHostingPlatform>()).Loaded.Should().BeTrue();
        }

        [Then]
        public void ShouldHaveHostApplicationLifetimeStarted()
        {
            Host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.IsCancellationRequested.Should().BeTrue();
        }

        [Then]
        public void ShouldHaveHostApplicationLifetimeStopping()
        {
            Host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.IsCancellationRequested.Should().BeTrue();
        }

        [Then]
        public void ShouldHaveHostApplicationLifetimeStopped()
        {
            Host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopped.IsCancellationRequested.Should().BeTrue();
        }
    }
}
