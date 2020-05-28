using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Hostly
{
    public class XamarinHostBuilderContext
    {
        public IHostEnvironment HostEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }
        public string RuntimePlatform { get; set; }
    }
}
