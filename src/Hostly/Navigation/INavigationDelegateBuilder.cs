using System;
using System.Threading.Tasks;

namespace Hostly.Navigation
{
    internal interface INavigationDelegateBuilder
    {
        bool TryProcessDelegate<TMiddleware, TDelegate>(TMiddleware middleware) 
            where TMiddleware : class
            where TDelegate : Delegate;

        void ProcessDelegate(Action<InsertPageBeforeDelegate, NavigationContext> action);
        void ProcessDelegate(Func<PushDelegate, NavigationContext, Task> func);
        void ProcessDelegate(Func<PopDelegate, NavigationContext, Task> func);
        void ProcessDelegate(Func<PushModalDelegate, NavigationContext, Task> func);
        void ProcessDelegate(Func<PopModalDelegate, NavigationContext, Task> func);
        void ProcessDelegate(Func<PopToRootDelegate, NavigationContext, Task> func);
        void ProcessDelegate(Action<RemovePageDelegate, NavigationContext> action);
        void GenerateProxies();
    }
}
