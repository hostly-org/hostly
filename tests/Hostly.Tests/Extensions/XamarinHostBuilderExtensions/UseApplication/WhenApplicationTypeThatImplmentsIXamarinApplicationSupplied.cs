using FluentAssertions;
using Hostly.Extensions;
using Hostly.Internals;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseApplication
{
    public class WhenApplicationTypeThatImplmentsIXamarinApplicationSupplied : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform(new MockPlatform())
                .UseApplication<MockXamarinApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationType()
        {
            Host.Services.GetRequiredService<XamarinApplicationDelegate>()().Should().BeOfType<MockXamarinApplication>();
        }
    }
}
