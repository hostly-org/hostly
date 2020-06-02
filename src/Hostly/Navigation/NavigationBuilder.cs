using System;
using System.Linq;
using System.Threading.Tasks;
using Hostly.Extensions;

namespace Hostly.Navigation
{
    internal sealed class NavigationBuilder : INavigationBuilder
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationDelegateBuilder _navigationDelegateBuilder;

        public NavigationBuilder(IServiceProvider serviceProvider, INavigationDelegateBuilder navigationDelegateBuilder)
        {
            _serviceProvider = serviceProvider;
            _navigationDelegateBuilder = navigationDelegateBuilder;
        }

        public void Build() => _navigationDelegateBuilder.BuildProxies();
        public void UseMiddleware(Action<InsertPageBeforeDelegate, NavigationContext> action) => _navigationDelegateBuilder.ProcessDelegate(action);
        public void UseMiddleware(Func<PushDelegate, NavigationContext, Task> func) => _navigationDelegateBuilder.ProcessDelegate(func);
        public void UseMiddleware(Func<PopDelegate, NavigationContext, Task> func) => _navigationDelegateBuilder.ProcessDelegate(func);
        public void UseMiddleware(Func<PushModalDelegate, NavigationContext, Task> func) => _navigationDelegateBuilder.ProcessDelegate(func);
        public void UseMiddleware(Func<PopModalDelegate, NavigationContext, Task> func) => _navigationDelegateBuilder.ProcessDelegate(func);
        public void UseMiddleware(Func<PopToRootDelegate, NavigationContext, Task> func) => _navigationDelegateBuilder.ProcessDelegate(func);
        public void UseMiddleware(Action<RemovePageDelegate, NavigationContext> action) => _navigationDelegateBuilder.ProcessDelegate(action);

        public void UseMiddleware<TMiddleware>() where TMiddleware : class
        {
            var constructor = typeof(TMiddleware).GetConstructors()
                .OrderByDescending(c => c.GetParameters())
                .First();

            var consturctorParameters = constructor.GetParameters();
            var @params = new object[consturctorParameters.Length];

            for (int i = 0; i < @params.Length; i++)
            {
                var currentParam = consturctorParameters[i];
                var value = _serviceProvider.GetService(currentParam.ParameterType);

                if(value == null && !currentParam.TryGetDefaultValue(out value))
                {
                    throw new InvalidOperationException($"Unable to resolve service for type `{currentParam.ParameterType}`");
                }

                @params[i] = value;
            }

            var middleware = (TMiddleware)constructor.Invoke(@params);

            if(!_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, InsertPageBeforeDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, PushDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, PopDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, PushModalDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, PopModalDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, PopToRootDelegate>(middleware)
                & !_navigationDelegateBuilder.TryProcessDelegate<TMiddleware, RemovePageDelegate>(middleware))
            {
                throw new InvalidOperationException($"Middleware of type: {typeof(TMiddleware)} must implement at least one navigation delegate");
            }
        }
    }
}
