using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenValidInstanceOfStartupTypeThatDoesNotImplementIXamarinStartupIsSupplied : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup(typeof(MockStartup));

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveServicesConfiguredFromStartup()
        {
            MockStartup.GlobalHasBeenConfigured.Should().BeTrue();
        }
    }
}
