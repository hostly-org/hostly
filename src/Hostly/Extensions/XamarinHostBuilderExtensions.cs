using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Hostly.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IXamarinHostBuilder"/>.
    /// </summary>
    public static class XamarinHostBuilderExtensions
    {
        /// <summary>
        /// Initializes <see cref="IConfiguration"/> with appsettings,json files.
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="assembly">The assembly that contains the embedded appsettings.json files.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseAppSettings(this IXamarinHostBuilder builder, Assembly assembly) 
        {
            return builder.ConfigureHostConfiguration(c =>
             {
                 c.AddEmbeddedJsonFile(assembly, "appsettings.json");

                 var environment = c.Build().GetValue<string>(XamarinHostDefaults.EnvironmentKey);

                 if(!string.IsNullOrEmpty(environment))
                 {
                     try
                     {
                         c.AddEmbeddedJsonFile(assembly, $"appsettings.{environment}.json");
                     }
                     catch (FileNotFoundException)
                     {
                         // At this point it just means an environment is supplied but no appsetting exists for it
                     }
                 }
             });
        }

        /// <summary>
        /// Initializes <see cref="IConfiguration"/> with appsettings,json files.
        /// </summary>
        /// <typeparam name ="TAssemblyClass">The type in the assembly that contains the embedded appsettings.json files</typeparam>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseAppSettings<TAssemblyClass>(this IXamarinHostBuilder builder)
        {
            return builder.UseAppSettings(typeof(TAssemblyClass).Assembly);
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinApplication"/>
        /// </summary>
        /// <typeparam name ="TApp">The type used to register <see cref="IXamarinApplication"/>. Must either be of type Application or IXamarinApplication</typeparam>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseApplication<TApp>(this IXamarinHostBuilder builder) where TApp : class, new()
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton(XamarinApplicationBuilder.Build<TApp>());
            });
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinApplication"/>
        /// </summary>
        /// <typeparam name ="TApp">The type used to register <see cref="IXamarinApplication"/></typeparam>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="app">The instance of <see cref="IXamarinApplication"/> to register.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseApplication<TApp>(this IXamarinHostBuilder builder, TApp app) where TApp : IXamarinApplication
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinApplication>(app);
            });
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinHostingPlatform"/>
        /// </summary>
        /// <typeparam name ="TPlatform">The type used to register <see cref="IXamarinHostingPlatform"/></typeparam>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UsePlatform<TPlatform>(this IXamarinHostBuilder builder) where TPlatform : class, IXamarinHostingPlatform
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinHostingPlatform, TPlatform>();
            });
        }

        /// <summary>
        /// Configures the host services with an insatnce of <see cref="IXamarinHostingPlatform"/>
        /// </summary>
        /// <typeparam name ="TPlatform">The type used to register <see cref="IXamarinHostingPlatform"/></typeparam>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="platform">The instance of <see cref="IXamarinHostingPlatform"/> to register.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UsePlatform<TPlatform>(this IXamarinHostBuilder builder, TPlatform platform) where TPlatform : IXamarinHostingPlatform
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinHostingPlatform>(platform);
            });
        }

        /// <summary>
        /// Configures the host services with a Startup class
        /// </summary>
        /// <param name="builder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="startupType">The class type to configure services with</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseStartup(this IXamarinHostBuilder builder, Type startupType)
        {
            return builder
                .ConfigureServices((context, services) =>
                {
                    if (typeof(IXamarinStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton(typeof(IXamarinStartup), startupType);
                    }
                    else
                    {
                        services.AddSingleton(typeof(IXamarinStartup), sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IXamarinHostEnvironment>();
                            return new XamarinStartup(StartupLoader.LoadMethods(sp, startupType, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        /// <summary>
        /// Configures the host services with a Startup class
        /// </summary>
        /// <typeparam name ="TStartup">The type used to configure services</typeparam>
        /// <param name="hostBuilder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <param name="startup">The instance of startup class to configure services with</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseStartup<TStartup>(this IXamarinHostBuilder hostBuilder, TStartup startup) where TStartup : class
        {
            var startupType = typeof(TStartup);
            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    if (typeof(IXamarinStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton((IXamarinStartup)startup);
                    }
                    else
                    {
                        services.AddSingleton<IXamarinStartup>(sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IXamarinHostEnvironment>();
                            return new XamarinStartup(StartupLoader.LoadMethods(startup, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        /// <summary>
        /// Configures the host services with a Startup class
        /// </summary>
        /// <typeparam name ="TStartup">The type used to configure services</typeparam>
        /// <param name="hostBuilder">The <see cref="IXamarinHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IXamarinHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseStartup<TStartup>(this IXamarinHostBuilder hostBuilder) where TStartup : class, new()
        {
            return hostBuilder.UseStartup(new TStartup());
        }
    }
}
