using Hostly.Extensions;
using Windows.UI.Xaml;

namespace Hostly.UWP.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IXamarinHostBuilder"/>.
    /// </summary>
    public static class XamarinHostBuilderExtensions
    {
        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinHostingPlatform"/>
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="app">The instance of <see cref="FormsApplicationDelegate"/> to register.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UsePlatform(this IXamarinHostBuilder builder, Application app)
        {
            return builder.UsePlatform(new UWPHostingPlatform(app));
        }
    }
}
