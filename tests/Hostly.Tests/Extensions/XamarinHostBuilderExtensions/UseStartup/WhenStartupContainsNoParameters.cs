using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenStartupContainsNoParameters : Specification
    {
        private class Startup
        {
            public static bool GlobalHasBeenConfigured { get; private set; }
            public void ConfigureServices()
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
        public void ShouldNotHaveServicesConfiguredFromStartup()
        {
            Startup.GlobalHasBeenConfigured.Should().BeTrue();
        }
    }
}
