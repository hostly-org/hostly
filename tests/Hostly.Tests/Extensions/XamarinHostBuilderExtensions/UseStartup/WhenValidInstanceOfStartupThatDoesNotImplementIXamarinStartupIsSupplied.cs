using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidInstanceOfStartupThatDoesNotImplementIXamarinStartupIsSupplied : Specification
    {
        private MockStartup _expectedStartup;

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _expectedStartup = new MockStartup();

            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup(_expectedStartup);

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            MockStartup.GlobalHasBeenConfigured.Should().BeTrue();
        }
    }
}
