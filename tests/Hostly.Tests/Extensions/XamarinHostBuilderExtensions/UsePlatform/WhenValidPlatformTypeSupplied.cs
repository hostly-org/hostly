using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UsePlatform
{
    public class WhenValidPlatformTypeSupplied : Specification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationType()
        {
            Host.Services.GetRequiredService<IXamarinHostingPlatform>().Should().BeOfType<MockPlatform>();
        }
    }
}
