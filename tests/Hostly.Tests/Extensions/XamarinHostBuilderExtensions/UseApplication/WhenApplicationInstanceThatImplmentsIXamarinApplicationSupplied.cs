using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Internals;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseApplication
{
    public class WhenApplicationInstanceThatImplmentsIXamarinApplicationSupplied : Specification
    {
        private MockXamarinApplication _expectedApplication;

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _expectedApplication = new MockXamarinApplication();

            _xamarinHostBuilder.UsePlatform(new MockPlatform())
                .UseApplication(_expectedApplication);

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedApplicationInstance()
        {
            Host.Services.GetRequiredService<XamarinApplicationDelegate>()().Should().Be(_expectedApplication);
        }
    }
}
