using FluentAssertions;
using Hostly.Extensions;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseApplication
{
    public class WhenValidApplicationInstanceSupplied : XamarinHostSpecification
    {
        private MockApplication _expectedApplication;

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _expectedApplication = new MockApplication();

            _xamarinHostBuilder.UsePlatform(new MockPlatform())
                .UseApplication(_expectedApplication);

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationInstance()
        {
            Host.Services.GetRequiredService<IXamarinApplication>().Should().Be(_expectedApplication);
        }
    }
}
