using Microsoft.Extensions.DependencyInjection;

namespace Hostly
{
    /// <summary>
    /// Represents a Startup file used to configure <see cref="IXamarinHost"/>
    /// </summary>
    public interface IXamarinStartup
    {
        /// <summary>
        /// Configures additional services for the <see cref="IXamarinHost"/>.
        /// </summary>
        /// <param name="ctx">The context of <see cref="IXamarinHostBuilder"/></param>
        /// <param name="services">The <see cref="IServiceCollection"/> to be configured</param>
        /// <returns><see cref="void"/>.</returns>
        void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services);
    }
}
