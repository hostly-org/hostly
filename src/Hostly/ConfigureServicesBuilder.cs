using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Hostly
{
    internal sealed class ConfigureServicesBuilder
    {
        public ConfigureServicesBuilder(MethodInfo configureServices)
        {
            MethodInfo = configureServices;
        }

        public MethodInfo MethodInfo { get; }

        public Action<XamarinHostBuilderContext, IServiceCollection> Build(object instance) => (context, services) => Invoke(instance, context, services);

        private void Invoke(object instance, XamarinHostBuilderContext context, IServiceCollection services)
        {
            if (MethodInfo == null)
            {
                return;
            }

            // Only support IServiceCollection parameters
            var parameters = MethodInfo.GetParameters();
            var numOfServiceParams = parameters.Count(p => p.ParameterType == typeof(IServiceCollection));
            var numOfContextParams = parameters.Count(p => p.ParameterType == typeof(XamarinHostBuilderContext));

            if(numOfServiceParams > 1)
                throw new InvalidOperationException($"The ConfigureServices method only supports one parameter of type {nameof(IServiceCollection)}.");
            if (numOfContextParams > 1)
                throw new InvalidOperationException($"The ConfigureServices method only supports one parameter of type {nameof(XamarinHostBuilderContext)}.");
            if(parameters.Length > 2 || (parameters.Length > 0 && numOfServiceParams == 0 && numOfContextParams == 0))
                throw new InvalidOperationException($"The ConfigureServices method only supports parameters of type {nameof(XamarinHostBuilderContext)} and {nameof(IServiceCollection)}.");

            var arguments = new object[MethodInfo.GetParameters().Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];

                if (param.ParameterType == typeof(IServiceCollection))
                    arguments[i] = services;

                if (param.ParameterType == typeof(XamarinHostBuilderContext))
                    arguments[i] = context;
            }

            MethodInfo.Invoke(instance, arguments);
        }
    }
}
