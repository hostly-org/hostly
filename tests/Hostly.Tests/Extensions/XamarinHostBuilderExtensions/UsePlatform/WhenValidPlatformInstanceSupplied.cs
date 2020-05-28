using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UsePlatform
{
    public class WhenValidPlatformInstanceSupplied : XamarinHostSpecification
    {
        private MockPlatform _expectedPlatform;

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _expectedPlatform = new MockPlatform();

            _xamarinHostBuilder.UsePlatform(_expectedPlatform)
                .UseApplication<MockApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationInstance()
        {
            Host.Services.GetRequiredService<IXamarinHostingPlatform>().Should().Be(_expectedPlatform);
        }
    }
}
