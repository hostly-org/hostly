using System;
using System.Collections.Generic;
using System.Text;

namespace Hostly
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(XamarinHostBuilderContext hostContext, object containerBuilder);
    }
}
