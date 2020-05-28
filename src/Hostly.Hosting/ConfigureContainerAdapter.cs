using System;

namespace Hostly
{
    internal sealed class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private Action<XamarinHostBuilderContext, TContainerBuilder> _action;

        public ConfigureContainerAdapter(Action<XamarinHostBuilderContext, TContainerBuilder> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void ConfigureContainer(XamarinHostBuilderContext hostContext, object containerBuilder)
        {
            _action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}
