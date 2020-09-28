using System;
using Xunit;
using Xunit.Sdk;

namespace Hostly.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [TraitDiscoverer("Hostly.Testing.Discoverers.IntegrationTraitDiscoverer", "Hostly.Testing")]
    public class IntegrationAttribute : FactAttribute, ITraitAttribute { }
}
