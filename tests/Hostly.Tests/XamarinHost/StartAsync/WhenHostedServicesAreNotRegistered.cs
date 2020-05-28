using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hostly.Tests.XamarinHost.StartAsync
{
    public class WhenHostedServicesAreNotRegistered : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Host.StartAsync();
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UseApplication<MockApplication>()
                .UsePlatform<MockPlatform>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldNotHaveAnyHostedServicesRegistered()
        {
            Host.Services.GetService<IEnumerable<IHostedService>>().Should().BeEmpty();
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
        public void ShouldNotHaveHostApplicationLifetimeStopping()
        {
            Host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.IsCancellationRequested.Should().BeFalse();
        }

        [Then]
        public void ShouldNotHaveHostApplicationLifetimeStopped()
        {
            Host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopped.IsCancellationRequested.Should().BeFalse();
        }
    }
}
