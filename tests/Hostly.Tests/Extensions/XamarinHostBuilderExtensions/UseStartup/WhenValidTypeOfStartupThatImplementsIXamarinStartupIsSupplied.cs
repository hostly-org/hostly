using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidTypeOfStartupThatImplementsIXamarinStartupIsSupplied : Specification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup<MockXamarinStartup>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedStartupType()
        {
            Host.Services.GetRequiredService<IXamarinStartup>().Should().BeOfType<MockXamarinStartup>();
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            var startup = (MockXamarinStartup)Host.Services.GetRequiredService<IXamarinStartup>();
            startup.HasBeenConfigured.Should().BeTrue();
        }
    }
}
