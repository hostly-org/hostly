using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenStartupContainsOnlyServicesParameter : Specification
    {
        private class Startup
        {
            public static bool GlobalHasBeenConfigured { get; private set; }
            public void ConfigureServices(IServiceCollection services)
            {
                GlobalHasBeenConfigured = true;
            }
        }

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup<Startup>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            Startup.GlobalHasBeenConfigured.Should().BeTrue();
        }
    }
}
