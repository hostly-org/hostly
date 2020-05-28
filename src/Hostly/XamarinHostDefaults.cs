namespace Hostly
{
    /// <summary>
    /// Represents default keys in <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> for configuring <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/>/>
    /// </summary>
    public static class XamarinHostDefaults
    {
        /// <summary>
        /// Represents the key in <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> for configuring <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName"/>/>
        /// </summary>
        public const string EnvironmentKey = "HOSTLY_ENVIRONMENT";
        /// <summary>
        /// Represents the key in <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> for configuring <see cref="Microsoft.Extensions.Hosting.IHostEnvironment.ApplicationName"/>/>
        /// </summary>
        public const string ApplicationKey = "HOSTLY_APPLICATION";
    }
}
