using System;
using System.Linq;
using Hostly.Extensions;

namespace Hostly.Internals
{
    internal static class Activator
    {
        public static T Activate<T>(IServiceProvider serviceProvider)
        {
            var constructor = typeof(T).GetConstructors()
                .OrderByDescending(c => c.GetParameters())
                .First();

            var consturctorParameters = constructor.GetParameters();
            var @params = new object[consturctorParameters.Length];

            for (int i = 0; i < @params.Length; i++)
            {
                var currentParam = consturctorParameters[i];
                var value = serviceProvider.GetService(currentParam.ParameterType);

                if (value == null && !currentParam.TryGetDefaultValue(out value))
                {
                    throw new InvalidOperationException($"Unable to resolve service for type `{currentParam.ParameterType}`");
                }

                @params[i] = value;
            }

            return (T)constructor.Invoke(@params);
        }
    }
}
