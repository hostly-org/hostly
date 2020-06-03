using System;
using System.Threading.Tasks;

namespace Hostly.Navigation
{
    internal sealed class NavigationBuilder : INavigationBuilder
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationProxyBuilder _navigationProxyBuilder;

        public NavigationBuilder(IServiceProvider serviceProvider, INavigationProxyBuilder navigationProxyBuilder)
        {
            _serviceProvider = serviceProvider;
            _navigationProxyBuilder = navigationProxyBuilder;
        }

        public void Build() => _navigationProxyBuilder.Build();
        public void UseMiddleware(Action<InsertPageBeforeDelegate, NavigationContext> action) => _navigationProxyBuilder.Process(action);
        public void UseMiddleware(Func<PushDelegate, NavigationContext, Task> func) => _navigationProxyBuilder.Process(func);
        public void UseMiddleware(Func<PopDelegate, NavigationContext, Task> func) => _navigationProxyBuilder.Process(func);
        public void UseMiddleware(Func<PushModalDelegate, NavigationContext, Task> func) => _navigationProxyBuilder.Process(func);
        public void UseMiddleware(Func<PopModalDelegate, NavigationContext, Task> func) => _navigationProxyBuilder.Process(func);
        public void UseMiddleware(Func<PopToRootDelegate, NavigationContext, Task> func) => _navigationProxyBuilder.Process(func);
        public void UseMiddleware(Action<RemovePageDelegate, NavigationContext> action) => _navigationProxyBuilder.Process(action);

        public void UseMiddleware<TMiddleware>() where TMiddleware : class
        {
            var middleware = Internals.Activator.Activate<TMiddleware>(_serviceProvider);

            if (!_navigationProxyBuilder.TryProcess<TMiddleware, InsertPageBeforeDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, PushDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, PopDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, PushModalDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, PopModalDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, PopToRootDelegate>(middleware)
                & !_navigationProxyBuilder.TryProcess<TMiddleware, RemovePageDelegate>(middleware))
            {
                throw new InvalidOperationException($"Middleware of type: {typeof(TMiddleware)} must implement at least one navigation delegate");
            }
        }
    }
}
