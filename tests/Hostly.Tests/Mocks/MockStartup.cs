using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Tests.Mocks
{
    public class MockStartup
    {
        public static bool GlobalHasBeenConfigured { get; private set; }

        public void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services)
        {
            GlobalHasBeenConfigured = true;
        }
    }
}
