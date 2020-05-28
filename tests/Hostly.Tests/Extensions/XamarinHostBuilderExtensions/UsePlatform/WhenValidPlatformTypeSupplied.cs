using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UsePlatform
{
    public class WhenValidPlatformTypeSupplied : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationType()
        {
            Host.Services.GetRequiredService<IXamarinHostingPlatform>().Should().BeOfType<MockPlatform>();
        }
    }
}
