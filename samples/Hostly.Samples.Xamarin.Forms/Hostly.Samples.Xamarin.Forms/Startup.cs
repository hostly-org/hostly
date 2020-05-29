using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Samples.Xamarin.Forms
{
    public class Startup
    {
        public void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services)
        {
            services.AddTransient(typeof(MainPage));
        }
    }
}
