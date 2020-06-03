using System;
using System.Threading.Tasks;

namespace Hostly.Navigation
{
    internal interface INavigationProxyBuilder
    {
        bool TryProcess<TMiddleware, TDelegate>(TMiddleware middleware) 
            where TMiddleware : class
            where TDelegate : Delegate;

        void Process(Action<InsertPageBeforeDelegate, NavigationContext> action);
        void Process(Func<PushDelegate, NavigationContext, Task> func);
        void Process(Func<PopDelegate, NavigationContext, Task> func);
        void Process(Func<PushModalDelegate, NavigationContext, Task> func);
        void Process(Func<PopModalDelegate, NavigationContext, Task> func);
        void Process(Func<PopToRootDelegate, NavigationContext, Task> func);
        void Process(Action<RemovePageDelegate, NavigationContext> action);
        void Build();
    }
}
