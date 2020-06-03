using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hostly.Internals;

namespace Hostly.Navigation
{
    internal class NavigationProxyBuilder : INavigationProxyBuilder
    {
        private readonly IList<Action<InsertPageBeforeDelegate, NavigationContext>> _insertBeforePageDelegates;
        private readonly IList<Func<PushDelegate, NavigationContext, Task>> _pushDelegates;
        private readonly IList<Func<PopDelegate, NavigationContext, Task>> _popDelegates;
        private readonly IList<Func<PushModalDelegate, NavigationContext, Task>> _pushModalDelegates;
        private readonly IList<Func<PopModalDelegate, NavigationContext, Task>> _popModalDelegates;
        private readonly IList<Func<PopToRootDelegate, NavigationContext, Task>> _popToRootDelegates;
        private readonly IList<Action<RemovePageDelegate, NavigationContext>> _removePageDelegates;

        private readonly Dictionary<Type, Action<object, MethodInfo>> _delegateActions;
        private readonly XamarinNavigationRootDelegate _xamarinNavigationDelegate;
        private readonly XamarinNavigationProxy _xamarinNavigationProxy;

        public NavigationProxyBuilder(XamarinNavigationRootDelegate xamarinNavigationDelegate,
            XamarinNavigationProxy xamarinNavigationProxy)
        {
            _xamarinNavigationDelegate = xamarinNavigationDelegate;
            _xamarinNavigationProxy = xamarinNavigationProxy;

            _insertBeforePageDelegates = new List<Action<InsertPageBeforeDelegate, NavigationContext>>();
            _pushDelegates = new List<Func<PushDelegate, NavigationContext, Task>>();
            _popDelegates = new List<Func<PopDelegate, NavigationContext, Task>>();
            _pushModalDelegates = new List<Func<PushModalDelegate, NavigationContext, Task>>();
            _popModalDelegates = new List<Func<PopModalDelegate, NavigationContext, Task>>();
            _popToRootDelegates = new List<Func<PopToRootDelegate, NavigationContext, Task>>();
            _removePageDelegates = new List<Action<RemovePageDelegate, NavigationContext>>();

            _delegateActions = new Dictionary<Type, Action<object, MethodInfo>>();

            _delegateActions.Add(typeof(InsertPageBeforeDelegate), HandleInsertBeforePage);
            _delegateActions.Add(typeof(PushDelegate), HandlePush);
            _delegateActions.Add(typeof(PopDelegate), HandlePop);
            _delegateActions.Add(typeof(PushModalDelegate), HandlePushModal);
            _delegateActions.Add(typeof(PopModalDelegate), HandlePopModal);
            _delegateActions.Add(typeof(PopToRootDelegate), HandlePopToRoot);
            _delegateActions.Add(typeof(RemovePageDelegate), HandleRemovePage);
        }

        public void Process(Action<InsertPageBeforeDelegate, NavigationContext> action) => _insertBeforePageDelegates.Add(action);
        public void Process(Func<PushDelegate, NavigationContext, Task> func) => _pushDelegates.Add(func);
        public void Process(Func<PopDelegate, NavigationContext, Task> func) => _popDelegates.Add(func);
        public void Process(Func<PushModalDelegate, NavigationContext, Task> func) => _pushModalDelegates.Add(func);
        public void Process(Func<PopModalDelegate, NavigationContext, Task> func) => _popModalDelegates.Add(func);
        public void Process(Func<PopToRootDelegate, NavigationContext, Task> func) => _popToRootDelegates.Add(func);
        public void Process(Action<RemovePageDelegate, NavigationContext> action) => _removePageDelegates.Add(action);
        private void HandleInsertBeforePage(object middleware, MethodInfo method) => _insertBeforePageDelegates.Add((next, ctx) => method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));
        private void HandlePush(object middleware, MethodInfo method) => _pushDelegates.Add((next, ctx) => (Task)method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));
        private void HandlePop(object middleware, MethodInfo method) => _popDelegates.Add(((next, ctx) => (Task)method.Invoke(middleware, BuildParams(ctx, method, next).ToArray())));
        private void HandlePushModal(object middleware, MethodInfo method) => _pushModalDelegates.Add((next, ctx) => (Task)method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));
        private void HandlePopModal(object middleware, MethodInfo method) => _popModalDelegates.Add((next, ctx) => (Task)method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));
        private void HandlePopToRoot(object middleware, MethodInfo method) => _popToRootDelegates.Add((next, ctx) => (Task)method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));
        private void HandleRemovePage(object middleware, MethodInfo method) => _removePageDelegates.Add((next, ctx) => method.Invoke(middleware, BuildParams(ctx, method, next).ToArray()));

        public bool TryProcess<TMiddleware, TDelegate>(TMiddleware middleware)
            where TMiddleware : class
            where TDelegate : Delegate
        {
            var delegateType = typeof(TDelegate);
            var methods = typeof(TMiddleware).GetMethods()
                .Where(m => m.GetParameters().Length > 0 && m.GetParameters().All(pi => pi.ParameterType == delegateType || pi.ParameterType == typeof(NavigationContext)))
                .ToArray();

            if (methods.Length == 0)
                return false;

            if (methods.Length > 1)
                throw new InvalidOperationException($"More than one methods on middleware: {typeof(TMiddleware).Name} implement delegate: {typeof(TDelegate).Name}");

            var methodParams = methods[0].GetParameters();

            if (methodParams.Length > 2)
                throw new InvalidOperationException($"'{methods[0].Name}' has invalid parameters, only single parameter of '{typeof(TDelegate).Name}' and '{typeof(NavigationContext).Name}' are allowed");

            Process<TDelegate>(middleware, methods[0]);

            return true;
        }

        private void Process<TDelegate>(object middleware, MethodInfo method)
        {
            if (_delegateActions.TryGetValue(typeof(TDelegate), out var action))
                action(middleware, method);
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

        public void Build()
        {
            InsertPageBeforeDelegate insertPageBeforeDelegate = ctx => { };
            PushDelegate pushDelegate = ctx => Task.CompletedTask;
            PopDelegate popDelegate = ctx => Task.CompletedTask;
            PushModalDelegate pushModalDelegate = ctx => Task.CompletedTask;
            PopModalDelegate popModalDelegate = ctx => Task.CompletedTask;
            PopToRootDelegate popToRootDelegate = ctx => Task.CompletedTask;
            RemovePageDelegate removePageDelegate = ctx => { };

            foreach (var @delegate in _insertBeforePageDelegates.Reverse())
                insertPageBeforeDelegate = (ctx) => @delegate(insertPageBeforeDelegate, ctx);
            foreach (var @delegate in _pushDelegates.Reverse())
                pushDelegate = (ctx) => @delegate(pushDelegate, ctx);
            foreach (var @delegate in _popDelegates.Reverse())
                popDelegate = (ctx) => @delegate(popDelegate, ctx);
            foreach (var @delegate in _pushModalDelegates.Reverse())
                pushModalDelegate = (ctx) => @delegate(pushModalDelegate, ctx);
            foreach (var @delegate in _popModalDelegates.Reverse())
                popModalDelegate = (ctx) => @delegate(popModalDelegate, ctx);
            foreach (var @delegate in _popToRootDelegates.Reverse())
                popToRootDelegate = (ctx) => @delegate(popToRootDelegate, ctx);
            foreach (var @delegate in _removePageDelegates.Reverse())
                removePageDelegate = (ctx) => @delegate(removePageDelegate, ctx);

            _xamarinNavigationProxy.Initialize(_xamarinNavigationDelegate());
            _xamarinNavigationProxy.InsertPageBeforeDelegate = insertPageBeforeDelegate;
            _xamarinNavigationProxy.PushDelegate = pushDelegate;
            _xamarinNavigationProxy.PopDelegate = popDelegate;
            _xamarinNavigationProxy.PushModalDelegate = pushModalDelegate;
            _xamarinNavigationProxy.PopModalDelegate = popModalDelegate;
            _xamarinNavigationProxy.PopToRootDelegate = popToRootDelegate;
            _xamarinNavigationProxy.RemovePageDelegate = removePageDelegate;
        }
    }
}
