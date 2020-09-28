using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidInstanceOfStartupThatImplementsIXamarinStartupIsSupplied : Specification
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
