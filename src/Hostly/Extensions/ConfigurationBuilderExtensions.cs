using Hostly.AppSettings;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Hostly.Extensions
{
    internal static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEmbeddedJsonFile(this IConfigurationBuilder cb,
            Assembly assembly, string name, bool optional = false)
        {
            return cb.AddJsonFile(new EmbeddedFileProvider(assembly), name, optional, false);
        }
    }
}
