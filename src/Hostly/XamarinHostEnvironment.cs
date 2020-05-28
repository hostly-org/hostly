using Microsoft.Extensions.FileProviders;

namespace Hostly
{
    internal sealed class XamarinHostEnvironment : IXamarinHostEnvironment
    {
        public string DevicePlatform { get; set; }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
