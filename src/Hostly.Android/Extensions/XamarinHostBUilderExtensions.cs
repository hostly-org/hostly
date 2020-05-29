using Hostly.Extensions;
using Xamarin.Forms.Platform.Android;

namespace Hostly.Android.Extensions
{
    public static class XamarinHostBUilderExtensions
    {
        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinHostingPlatform"/>
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="app">The instance of <see cref="FormsAppCompatActivity"/> to register.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UsePlatform(this IXamarinHostBuilder builder, FormsAppCompatActivity app)
        {
            return builder.UsePlatform(new AndroidHostingPlatform(app));
        }
    }
}
