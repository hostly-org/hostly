using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hostly.Tests.XamarinHost.StartAsync
{
    public class WhenHostedServicesAreNotRegistered : Specification
    {
        protected override Task Given()
        {
            return Host.StartAsync();
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UseApplication<MockXamarinApplication>()
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
