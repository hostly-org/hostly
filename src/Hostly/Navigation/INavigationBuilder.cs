using System;
using System.Threading.Tasks;

namespace Hostly.Navigation
{
    public interface INavigationBuilder
    {
        void Build();
        void UseMiddleware(Action<InsertPageBeforeDelegate, NavigationContext> action);
        void UseMiddleware(Func<PushDelegate, NavigationContext, Task> func);
        void UseMiddleware(Func<PopDelegate, NavigationContext, Task> func);
        void UseMiddleware(Func<PushModalDelegate, NavigationContext, Task> func);
        void UseMiddleware(Func<PopModalDelegate, NavigationContext, Task> func);
        void UseMiddleware(Func<PopToRootDelegate, NavigationContext, Task> func);
        void UseMiddleware(Action<RemovePageDelegate, NavigationContext> func);
        void UseMiddleware<TMiddleware>() where TMiddleware : class;
    }
}
