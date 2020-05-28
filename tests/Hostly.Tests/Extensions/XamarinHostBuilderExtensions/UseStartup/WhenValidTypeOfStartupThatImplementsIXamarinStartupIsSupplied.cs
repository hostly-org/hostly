using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidTypeOfStartupThatImplementsIXamarinStartupIsSupplied : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockApplication>()
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
