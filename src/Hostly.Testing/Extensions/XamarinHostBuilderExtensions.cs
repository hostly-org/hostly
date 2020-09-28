using Hosting.Testing.Abstractions;
using Hostly.Extensions;
using Xamarin.Forms;

namespace Hostly.Testing.Extensions
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
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseTestPlatform<THostingPlatform>(this IXamarinHostBuilder builder)
            where THostingPlatform : class, IXamarinHostingPlatform
        {
            return builder.UseTestPlatformServices()
                .UsePlatform<THostingPlatform>();
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinHostingPlatform"/>
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseTestPlatform(this IXamarinHostBuilder builder)
        {
            return builder.UseTestPlatformServices()
                .UsePlatform<TestHostingPlatform>();
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="Xamarin.Forms.Internals.IPlatformServices"/>
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseTestPlatformServices(this IXamarinHostBuilder builder)
        {
            Device.PlatformServices = new TestPlatformServices();
            return builder;
        }
    }
}
