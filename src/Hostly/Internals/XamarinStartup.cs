using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;

namespace Hostly.Internals
{
    internal sealed class XamarinStartup : IXamarinStartup
    {
        private readonly StartupMethods _methods;

        public XamarinStartup(StartupMethods methods)
        {
            _methods = methods;
        }

        public void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services)
        {
            try
            {
                _methods.ConfigureServicesDelegate(ctx, services);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }

                throw;
            }
        }
    }
}
