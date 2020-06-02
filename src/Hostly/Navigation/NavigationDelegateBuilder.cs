using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hostly.Internals;

namespace Hostly.Navigation
{
    internal class NavigationDelegateBuilder : INavigationDelegateBuilder
    {
        private readonly Stack<InsertPageBeforeDelegate> _insertBeforePageDelegates;
        private readonly Stack<PushDelegate> _pushDelegates;
        private readonly Stack<PopDelegate> _popDelegates;
        private readonly Stack<PushModalDelegate> _pushModalDelegates;
        private readonly Stack<PopModalDelegate> _popModalDelegates;
        private readonly Stack<PopToRootDelegate> _popToRootDelegates;
        private readonly Stack<RemovePageDelegate> _removePageDelegates;

        private readonly Dictionary<Type, Action<object, MethodInfo>> _delegateActions;

        public NavigationDelegateBuilder(IServiceProvider serviceProvider)
        {
            _insertBeforePageDelegates = new Stack<InsertPageBeforeDelegate>();
            _pushDelegates = new Stack<PushDelegate>();
            _popDelegates = new Stack<PopDelegate>();
            _pushModalDelegates = new Stack<PushModalDelegate>();
            _popModalDelegates = new Stack<PopModalDelegate>();
            _popToRootDelegates = new Stack<PopToRootDelegate>();
            _removePageDelegates = new Stack<RemovePageDelegate>();

            _delegateActions = new Dictionary<Type, Action<object, MethodInfo>>();

            _delegateActions.Add(typeof(InsertPageBeforeDelegate), HandleInsertBeforePage);
            _delegateActions.Add(typeof(PushDelegate), HandlePush);
            _delegateActions.Add(typeof(PopDelegate), HandlePop);
            _delegateActions.Add(typeof(PushModalDelegate), HandlePushModal);
            _delegateActions.Add(typeof(PopModalDelegate), HandlePopModal);
            _delegateActions.Add(typeof(PopToRootDelegate), HandlePopToRoot);
            _delegateActions.Add(typeof(RemovePageDelegate), HandleRemovePage);
        }

        public void ProcessDelegate(Action<InsertPageBeforeDelegate, NavigationContext> action)
        {
            var previous = _insertBeforePageDelegates.Count > 0 ? _insertBeforePageDelegates.Pop() : (ctx => { });
            var @delegate = (InsertPageBeforeDelegate)(ctx => action(previous, ctx));
            _insertBeforePageDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Func<PushDelegate, NavigationContext, Task> func)
        {
            var previous = _pushDelegates.Count > 0 ? _pushDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PushDelegate)(ctx => func(previous, ctx));
            _pushDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Func<PopDelegate, NavigationContext, Task> func)
        {
            var previous = _popDelegates.Count > 0 ? _popDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopDelegate)(ctx => func(previous, ctx));
            _popDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Func<PushModalDelegate, NavigationContext, Task> func)
        {
            var previous = _pushModalDelegates.Count > 0 ? _pushModalDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PushModalDelegate)(ctx => func(previous, ctx));
            _pushModalDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Func<PopModalDelegate, NavigationContext, Task> func)
        {
            var previous = _popModalDelegates.Count > 0 ? _popModalDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopModalDelegate)(ctx => func(previous, ctx));
            _popModalDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Func<PopToRootDelegate, NavigationContext, Task> func)
        {
            var previous = _popToRootDelegates.Count > 0 ? _popToRootDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopToRootDelegate)(ctx => func(previous, ctx));
            _popToRootDelegates.Push(@delegate);
        }

        public void ProcessDelegate(Action<RemovePageDelegate, NavigationContext> action)
        {
            var previous = _removePageDelegates.Count > 0 ? _removePageDelegates.Pop() : (ctx => { });
            var @delegate = (RemovePageDelegate)(ctx => action(previous, ctx));
            _removePageDelegates.Push(@delegate);
        }

        public bool TryProcessDelegate<TMiddleware, TDelegate>(TMiddleware middleware)
            where TMiddleware : class
            where TDelegate : Delegate
        {
            var delegateType = typeof(TDelegate);
            var methods = typeof(TMiddleware).GetMethods()
                .Where(m => m.GetParameters().Length > 0 && m.GetParameters().Any(pi => pi.ParameterType == delegateType))
                .ToArray();

            if (methods.Length == 0)
                return false;

            if (methods.Length > 1)
                throw new InvalidOperationException($"More than one methods on middleware: {typeof(TMiddleware).Name} implement delegate: {typeof(TDelegate).Name}");

            var methodParams = methods[0].GetParameters();

            if (methodParams.Length > 2 || methodParams.Any(p => p.ParameterType != typeof(NavigationContext)))
                throw new InvalidOperationException($"'{methods[0].Name}' has invalid parameters, only types of '{typeof(TDelegate).Name}' and '{typeof(NavigationContext).Name}' are valid");

            ProcessDelegate<TDelegate>(middleware, methods[0]);

            return true;
        }

        private void ProcessDelegate<TDelegate>(object middleware, MethodInfo method)
        {
            if (_delegateActions.TryGetValue(typeof(TDelegate), out var action))
                action(middleware, method);
        }

        public void GenerateProxies()
        {
            XamarinProxies.NavigationProxy.InsertPageBeforeDelegate = _insertBeforePageDelegates.Count > 0 ? _insertBeforePageDelegates.Pop() : (ctx => { });
            XamarinProxies.NavigationProxy.PushDelegate = _pushDelegates.Count > 0 ? _pushDelegates.Pop() : (ctx => Task.CompletedTask);
            XamarinProxies.NavigationProxy.PopDelegate = _popDelegates.Count > 0 ? _popDelegates.Pop() : (ctx => Task.CompletedTask);
            XamarinProxies.NavigationProxy.PushModalDelegate = _pushModalDelegates.Count > 0 ? _pushModalDelegates.Pop() : (ctx => Task.CompletedTask);
            XamarinProxies.NavigationProxy.PopModalDelegate = _popModalDelegates.Count > 0 ? _popModalDelegates.Pop() : (ctx => Task.CompletedTask);
            XamarinProxies.NavigationProxy.PopToRootDelegate = _popToRootDelegates.Count > 0 ? _popToRootDelegates.Pop() : (ctx => Task.CompletedTask);
            XamarinProxies.NavigationProxy.RemovePageDelegate = _removePageDelegates.Count > 0 ? _removePageDelegates.Pop() : (ctx => { });
        }

        private void HandleInsertBeforePage(object middleware, MethodInfo method)
        {
            var previous = _insertBeforePageDelegates.Count > 0 ? _insertBeforePageDelegates.Pop() : (ctx => { });
            var @delegate = (InsertPageBeforeDelegate)(ctx => method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _insertBeforePageDelegates.Push(@delegate);
        }

        private void HandlePush(object middleware, MethodInfo method)
        {
            var previous = _pushDelegates.Count > 0 ? _pushDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PushDelegate)(ctx => (Task)method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _pushDelegates.Push(@delegate);
        }

        private void HandlePop(object middleware, MethodInfo method)
        {
            var previous = _popDelegates.Count > 0 ? _popDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopDelegate)(ctx => (Task)method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _popDelegates.Push(@delegate);
        }

        private void HandlePushModal(object middleware, MethodInfo method)
        {
            var previous = _pushModalDelegates.Count > 0 ? _pushModalDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PushModalDelegate)(ctx => (Task)method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _pushModalDelegates.Push(@delegate);
        }

        private void HandlePopModal(object middleware, MethodInfo method)
        {
            var previous = _popModalDelegates.Count > 0 ? _popModalDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopModalDelegate)(ctx => (Task)method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _popModalDelegates.Push(@delegate);
        }

        private void HandlePopToRoot(object middleware, MethodInfo method)
        {
            var previous = _popToRootDelegates.Count > 0 ? _popToRootDelegates.Pop() : (ctx => Task.CompletedTask);
            var @delegate = (PopToRootDelegate)(ctx => (Task)method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _popToRootDelegates.Push(@delegate);
        }

        private void HandleRemovePage(object middleware, MethodInfo method)
        {
            var previous = _removePageDelegates.Count > 0 ? _removePageDelegates.Pop() : (ctx => { });
            var @delegate = (RemovePageDelegate)(ctx => method.Invoke(middleware, BuildParams(ctx, method, previous).ToArray()));
            _removePageDelegates.Push(@delegate);
        }

        private IEnumerable<object> BuildParams(NavigationContext context, MethodInfo methodInfo, object arg)
        {
            foreach (var param in methodInfo.GetParameters())
            {
                if (param.ParameterType == typeof(NavigationContext))
                    yield return context;
                else
                    yield return arg;
            }
        }
    }
}
