using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Mocks
{
    public class MockXamarinStartup : IXamarinStartup
    {
        public static bool GlobalHasBeenConfigured { get; private set; }
        public bool HasBeenConfigured { get; private set; }

        public void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services)
        {
            GlobalHasBeenConfigured = true;
            HasBeenConfigured = true;
        }
    }
}
