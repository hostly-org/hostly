using Microsoft.Extensions.Hosting;

namespace Hostly
{
    /// <summary>
    /// Represents environment for <see cref="IXamarinHost"/>.
    /// </summary>
    public interface IXamarinHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Represents the platform the <see cref="IXamarinHost"/> is being executed on/>
        /// </summary>
        string DevicePlatform { get; set; }
    }
}
