using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UsePlatform
{
    public class WhenValidPlatformInstanceSupplied : Specification
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
                .UseApplication<MockXamarinApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationInstance()
        {
            Host.Services.GetRequiredService<IXamarinHostingPlatform>().Should().Be(_expectedPlatform);
        }
    }
}
