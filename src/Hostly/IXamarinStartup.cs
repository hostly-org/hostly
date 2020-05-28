using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly
{
    public interface IXamarinStartup
    {
        void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services);
    }
}
