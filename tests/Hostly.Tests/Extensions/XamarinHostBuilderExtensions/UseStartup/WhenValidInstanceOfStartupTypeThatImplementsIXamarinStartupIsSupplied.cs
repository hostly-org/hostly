using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidInstanceOfStartupTypeThatImplementsIXamarinStartupIsSupplied : Specification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup(typeof(MockXamarinStartup));

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            MockXamarinStartup.GlobalHasBeenConfigured.Should().BeTrue();
        }
    }
}
