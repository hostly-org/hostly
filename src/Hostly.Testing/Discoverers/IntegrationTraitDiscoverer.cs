using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Hostly.Testing.Discoverers
{
    public class IntegrationTraitDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>("Category", "Integration");
        }
    }
}
