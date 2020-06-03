using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Internals;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseApplication
{
    public class WhenApplicationTypeThatDoesNotImplmentIXamarinApplicationSupplied : XamarinHostSpecification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform(new MockPlatform())
                .UseApplication<MockApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationType()
        {
            Host.Services.GetRequiredService<XamarinApplicationDelegate>()().Should().BeAssignableTo<MockApplication>();
        }
    }
}
