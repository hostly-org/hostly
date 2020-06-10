using System;

namespace Hostly.Internals
{
    internal sealed class XamarinApplicationDelegateFactory
    {
        private IServiceProvider _serviceProvider;
        private XamarinApplicationDelegate _delegate;

        public void Init(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public XamarinApplicationDelegate Create<TApp>() where TApp : class
        {
            if (_delegate != null)
                return _delegate;

            var instance = Activator.Activate<TApp>(_serviceProvider);
            _delegate = () => instance;

            return _delegate;
        }
    }
}
