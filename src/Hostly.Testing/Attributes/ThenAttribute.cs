using System;
using Xunit;
using Xunit.Sdk;

namespace Hostly.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [TraitDiscoverer("Hostly.Testing.Discoverers.ThenTraitDiscoverer", "Hostly.Testing")]
    public class ThenAttribute : FactAttribute, ITraitAttribute { }
}
