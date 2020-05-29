using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidInstanceOfStartupThatImplementsIXamarinStartupIsSupplied : XamarinHostSpecification
    {
        private MockXamarinStartup _expectedStartup;

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _expectedStartup = new MockXamarinStartup();

            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup(_expectedStartup);

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedStartupInstance()
        {
            Host.Services.GetRequiredService<IXamarinStartup>().Should().Be(_expectedStartup);
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            var startup = (MockXamarinStartup)Host.Services.GetRequiredService<IXamarinStartup>();
            startup.HasBeenConfigured.Should().BeTrue();
        }
    }
}
