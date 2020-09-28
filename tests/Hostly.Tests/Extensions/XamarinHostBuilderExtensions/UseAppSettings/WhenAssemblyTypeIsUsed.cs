using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseAppSettings
{
    public class WhenAssemblyTypeIsUsed : Specification
    {
        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseAppSettings<MockStartup>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldHaveExpectedEnvironment()
        {
            var hostEnvironment = Host.Services.GetRequiredService<IXamarinHostEnvironment>();
            hostEnvironment.EnvironmentName.Should().Be("test");
        }

        [Then]
        public void ShouldHaveExpectedApplicationName()
        {
            var hostEnvironment = Host.Services.GetRequiredService<IXamarinHostEnvironment>();
            hostEnvironment.ApplicationName.Should().Be("hostly.test");
        }

        [Then]
        public void ShouldHaveExpectedConnectionString()
        {
            var configuration = Host.Services.GetRequiredService<IConfiguration>();
            configuration.GetConnectionString("default").Should().Be("hostly-test.db");
        }
    }
}
