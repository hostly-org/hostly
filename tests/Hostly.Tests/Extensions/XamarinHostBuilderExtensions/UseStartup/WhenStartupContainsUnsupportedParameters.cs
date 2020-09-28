using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hostly.Extensions;
using Hostly.Testing.Abstractions;
using Hostly.Testing.Attributes;
using Hostly.Tests.Mocks;

namespace Hostly.Tests.Extensions.XamarinHostBuilderExtensions.UseStartup
{
    public class WhenStartupContainsUnsupportedParameters : Specification
    {
        private class Startup
        {
            public static bool GlobalHasBeenConfigured { get; private set; }
            public void ConfigureServices(string test)
            {
                GlobalHasBeenConfigured = true;
            }
        }

        protected override Task Given()
        {
            return Task.CompletedTask;
        }

        protected override Task When()
        {
            ExceptionMode = ExceptionMode.Record;

            _xamarinHostBuilder.UsePlatform<MockPlatform>()
                .UseApplication<MockXamarinApplication>()
                .UseStartup<Startup>();

            return Task.CompletedTask;
        }

        [Then]
        public void ShouldNotHaveServicesConfiguredFromStartup()
        {
            Startup.GlobalHasBeenConfigured.Should().BeFalse();
        }

        [Then]
        public void InvalidOperationExceptionShouldBeThrown()
        {
            Exception.Should().BeOfType<InvalidOperationException>();
        }
    }
}
