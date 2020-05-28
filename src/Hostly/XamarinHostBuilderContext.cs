using Microsoft.Extensions.Configuration;

namespace Hostly
{
    /// <summary>
    /// Represents the context for <see cref="IXamarinHostBuilder"/>/>
    /// </summary>
    public class XamarinHostBuilderContext
    {
        /// <summary>
        /// Represents the <see cref="IXamarinHostEnvironment"/> for <see cref="IXamarinHostBuilder"/>/>
        /// </summary>
        public IXamarinHostEnvironment HostEnvironment { get; set; }
        /// <summary>
        /// Represents the <see cref="IConfiguration"/> for <see cref="IXamarinHostBuilder"/>/>
        /// </summary>
        public IConfiguration Configuration { get; set; }
    }
}
