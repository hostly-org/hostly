using Microsoft.Extensions.Hosting;

namespace Hostly
{
    public interface IXamarinHostEnvironment : IHostEnvironment
    {
        /// <summary>
        /// Represents the platform the <see cref="IXamarinHost"/> is being executed on/>
        /// </summary>
        string DevicePlatform { get; set; }
    }
}
